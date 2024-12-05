using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace DATN_Infrastructure.Repository
{
    public class OrderReponsitory : GenericeReponsitory<Order>, IOrderReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OrderReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(CreateOrder orderDTO)
        {
            var or = new Order
            {
                Total = orderDTO.Total,
                TimeOrder = DateTime.Now,
                StatusOrder = (byte)orderDTO.StatusOrder,
                PaymentMethod = orderDTO.PaymentMethod,
                VoucherId = orderDTO.VoucherId,
                AccountId = orderDTO.AccountId,
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Orders.Add(or);
                await _context.SaveChangesAsync();

                // Log the add action
                var log = new Login
                {
                    AccountId = orderDTO.AccountId, // Example: account that performed the action, change as needed
                    Action = "Thêm Order",
                    TimeStamp = DateTime.Now,
                    Description = $"Order '{or.Id}' đã được tạo."
                };

                await _context.Logins.AddAsync(log);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return or.Id; // Return the generated Order ID
            }
            catch
            {
                await transaction.RollbackAsync();
                return 0; // Return 0 in case of an error
            }
        }


        public async Task<bool> UpdateOrder(int id, UpdateOrder orderDTO)
        {
            var or = await _context.Orders.FindAsync(id);
            if (or != null)
            {
                or.StatusOrder = (byte)orderDTO.StatusOrder;

                // If there's a reason, set it on the order
                if (!string.IsNullOrEmpty(orderDTO.Reason))
                {
                    or.Reason = orderDTO.Reason;
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Orders.Update(or);
                    await _context.SaveChangesAsync();

                    Account admin = _context.Accounts.FirstOrDefault(a => a.Role == 1);
                    // Log the add action
                    var log = new Login
                    {
                        AccountId = admin.Id, // Example: account that performed the action, change as needed
                        Action = "Cập nhật đơn hàng", // Action description
                        TimeStamp = DateTime.Now,
                        Description = $"Order '{orderDTO.StatusOrder}' đã được sửa. Lý do: {orderDTO.Reason ?? "Không có lý do"}"
                    };

                    await _context.Logins.AddAsync(log);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            return false;
        }

        //public async Task<bool> DeleteCart(int id)
        //{
        //    var gh = await _context.DetailCarts.FindAsync(id);
        //    if (gh != null)
        //    {

        //        _context.DetailCarts.Remove(gh);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<ReturnOrder> GetAllAsync()
        {
            var result = new ReturnOrder();

            // Fetch orders including details
            var orders = await _context.Orders
                .Include(o => o.DetailOrder)
                .Include(o => o.Account)
                .Include(a => a.DeliveryAddress)
                .Include(b => b.DetailOrder)
                .ToListAsync();

            var data = orders.Select(item => new OrderDTO
            {
                Id = item.Id,
                OrderCode = $"DH{item.Id.ToString().PadLeft(4, '0')}",
                AccountId = item.Account.Id,
                VoucherId = item.VoucherId,
                Name = item.Account.FullName,
                Total = item.Total,
                PaymentMethod = item.PaymentMethod,
                StatusOrder = item.StatusOrder,
                TimeOrder = item.TimeOrder,
                OrderStatus = item.StatusOrder switch
                {
                    1 => "pending",
                    2 => "processing",
                    3 => "shipped",
                    4 => "completed",
                    _ => "cancelled"
                }
            }).ToList();

            // Map orders to OrderDTO
            result.Orders = data;
            return result;
        }

        public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Orders.Include(o => o.Voucher).Include(a => a.Account).ToListAsync();
        }

        public async Task<List<OrderUserDtos>> GetOrderByIdUser(int idUser)
        {
            List<Order> orders = await _context.Orders.Where(a => a.AccountId == idUser)
                                .Include(a => a.DeliveryAddress)
                                .Include(a => a.Account).Include(a => a.DetailOrder)
                                .ToListAsync();

            List<OrderUserDtos> result = orders.Select(item => new OrderUserDtos
            {
                Id = item.Id,
                OrderCode = $"DH{item.Id.ToString().PadLeft(4, '0')}",
                FullName = item.Account.FullName,
                NumberPhone = item.DeliveryAddress.FirstOrDefault(a => a.OrderId == item.Id).Phone,
                Status = item.StatusOrder,
                Address = item.DeliveryAddress.FirstOrDefault(a => a.OrderId == item.Id).Address,
            }).ToList();

            return result;
        }

        public async Task<OrderUserForDetailDtos> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Where(x => x.Id == id)
                .Include(a => a.Voucher)            
                .Include(a => a.DeliveryAddress)   
                .Include(a => a.Account)           
                .FirstOrDefaultAsync();             

            if (order == null)
            {
                return null;  
            }

            var detailOrder = await _context.DetailOrders
                .Where(x => x.OrderId == order.Id)
                .Include(a => a.DetailProduct)  
                .ThenInclude(dp => dp.Product).ThenInclude(dp=>dp.Media) 
                .ToListAsync();

            var deliveryAddress = order.DeliveryAddress?.FirstOrDefault();

            // Ánh xạ các DetailOrder thành DTO
            var detailOrderDtos = detailOrder.Select(detailOrder => new DetailOrderDtoForOrder
            {
                DetailProductId = detailOrder.DetailProductId, 
                Quantity = detailOrder.Quantity,             
                OrderId = detailOrder.OrderId,                 
                DetailProduct = _mapper.Map<ProductDetailDTO>(detailOrder.DetailProduct), 
                Product = _mapper.Map<ProductDTO>(detailOrder.DetailProduct.Product),    
            }).ToList();

            var result = new OrderUserForDetailDtos
            {
                Id = order.Id,
                OrderCode = $"DH{order.Id.ToString().PadLeft(4, '0')}",
                FullName = order.Account?.FullName, 
                NumberPhone = deliveryAddress?.Phone, 
                Status = order.StatusOrder,
                Address = deliveryAddress?.Address,
                Voucher = order.Voucher,
                DetailOrder = detailOrderDtos
            };

            return result;
        }





    }
}
