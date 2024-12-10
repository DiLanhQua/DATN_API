﻿using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography;
using System.Text;
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

                    if(or.StatusOrder == 4)
                    {
                        await UpdateQuantityProducts(id); // trừ số lượng khi giao hàng thành công
                    }

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

        private async Task UpdateQuantityProducts(int idOrder)
        {
            List<DetailOrder> detailOrders = await _context.DetailOrders.Where(a => a.OrderId == idOrder).ToListAsync();

            foreach(var item in detailOrders)
            {
                DetailProduct detailProduct = await _context.DetailProducts.FindAsync(item.DetailProductId);

                detailProduct.Quantity = detailProduct.Quantity - item.Quantity;

                _context.DetailProducts.Update(detailProduct);
            }

            await _context.SaveChangesAsync();
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
                    5 => "cancelled",
                    _ => "cc"
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
                                .Include(a => a.Account)
                                .Include(a => a.DetailOrder)
                                .ToListAsync();

            List<OrderUserDtos> result = new List<OrderUserDtos>();

            foreach (var item in  orders)
            {
                DeliveryAddress deliveryAddress = await _context.DeliveryAddresses.FirstOrDefaultAsync(a => a.OrderId == item.Id);

                OrderUserDtos order = new OrderUserDtos
                {
                    Id = item.Id,
                    OrderCode = $"DH{item.Id.ToString().PadLeft(4, '0')}",
                    FullName = item.Account.FullName,
                    NumberPhone = deliveryAddress.Phone ?? "Không có",
                    Status = item.StatusOrder,
                    Address = deliveryAddress.Address ?? "Không có",
                    Paymend = item.PaymentMethod
                };

                result.Add(order);
            }
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
                .ToListAsync();

            var deliveryAddress = order.DeliveryAddress?.FirstOrDefault();

            List<DetailOrderDtoForOrder> detailOrderRes = new List<DetailOrderDtoForOrder>();

            foreach ( var item in detailOrder)
            {
                var media = await _context.Medium
                .Where(a => a.ProductId == item.DetailProduct.ProductId && a.IsPrimary == true)
                .Join(
                    _context.Images,
                    medium => medium.ImagesId,
                    image => image.Id,
                    (medium, image) => new ImageDeDTO
                    {
                        Id = medium.Id,
                        IsImage = medium.IsPrimary,
                        Link = image.Link
                    }
                )
                .FirstOrDefaultAsync();

                List<MediaADD> mediaADDs = new List<MediaADD>();

                mediaADDs.Add(new MediaADD
                {
                    Link = media.Link,
                    IsPrimary = media.IsImage,
                });

                var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == item.DetailProduct.ProductId);

                ProductDTO productDTO = new ProductDTO
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    BrandId = product.BrandId,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    Medias = mediaADDs,
                };

                DetailOrderDtoForOrder detailOrderDtos = new DetailOrderDtoForOrder
                {
                    DetailProductId = item.DetailProductId,
                    Quantity = item.Quantity,
                    OrderId = item.OrderId,
                    DetailProduct = _mapper.Map<ProductDetailDTO>(item.DetailProduct),
                    Product = productDTO
                };

                detailOrderRes.Add(detailOrderDtos);
            }

            // Ánh xạ các DetailOrder thành DTO

            var result = new OrderUserForDetailDtos
            {
                Id = order.Id,
                OrderCode = $"DH{order.Id.ToString().PadLeft(4, '0')}",
                FullName = order.Account?.FullName, 
                NumberPhone = deliveryAddress?.Phone, 
                Status = order.StatusOrder,
                Address = deliveryAddress?.Address,
                Voucher = _mapper.Map<VoucherDTO>(order.Voucher) ,
                DetailOrder = detailOrderRes,
                TotalPrice=order.Total,
            };

            return result;
        }

        public string OrderPayVNPay(int amount, int id)
        {
            var vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";

            var vnp_Returnurl = "https://localhost:7048/api/Order/vnpay-return";

            var vnp_TmnCode = "MK3OFAHT";

            var vnp_HashSecret = "4IDW7MDQYS04GE2J50VCN2XS7QJ27KPN";

            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");

            vnpay.AddRequestData("vnp_Command", "pay");

            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);

            vnpay.AddRequestData("vnp_Amount", $"{(amount * 100)}");

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            vnpay.AddRequestData("vnp_CurrCode", "VND");

            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");

            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", id.ToString());

            vnpay.AddRequestData("vnp_OrderType", "other");

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);

            vnpay.AddRequestData("vnp_TxnRef", id.ToString());

            return vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        }

        public async Task<bool> AffterBanking(int id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            order.PaymentMethod = $"Online - Đã thanh toán";

            _context.Orders.Update(order);

            await _context.SaveChangesAsync();

            return true;
        }
    }

    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>();
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData)
            {
                if (data.Length > 0)
                {
                    data.Append("&");
                }
                data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
            }

            var rawData = data.ToString();
            var signData = HmacSHA512(hashSecret, rawData);
            var paymentUrl = $"{baseUrl}?{rawData}&vnp_SecureHash={signData}";
            return paymentUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var data = new StringBuilder();
            foreach (var kv in _responseData)
            {
                if (kv.Key != "vnp_SecureHash")
                {
                    if (data.Length > 0)
                    {
                        data.Append("&");
                    }
                    data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
                }
            }

            var rawData = data.ToString();
            var myChecksum = HmacSHA512(secretKey, rawData);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string HmacSHA512(string key, string inputData)
        {
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
