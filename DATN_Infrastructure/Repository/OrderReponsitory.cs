using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> AddAsync(CreateOrder orderDTO)
        {
            var or = new Order
            {
                Total = orderDTO.Total,
                TimeOrder = DateTime.Now,
                StatusOrder = orderDTO.StatusOrder,
                PaymentMethod = orderDTO.PaymentMethod,
                VoucherId = orderDTO.VoucherId,
                AccountId = orderDTO.AccountId,
            };
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                _context.Orders.Add(or);
                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                return true;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                throw new Exception("Đã xảy ra lỗi khi thêm đơn hàng.", ex);

            }
        }

        public async Task<bool> UpdateOrder(int id, UpdateOrder orderDTO)
        {
            var or = await _context.Orders.FindAsync(id);
            if (or != null)
            {
                or.StatusOrder = orderDTO.StatusOrder;

                _context.Orders.Update(or);
                await _context.SaveChangesAsync();
                return true;
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
                .Include(o => o.DetailOrder) // Include DetailOrders for eager loading
                .AsNoTracking()
                .ToListAsync();

            // Map orders to OrderDTO
            result.Orders = _mapper.Map<List<OrderDTO>>(orders);

            return result;
        }
    }
}
