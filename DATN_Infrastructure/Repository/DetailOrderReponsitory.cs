using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using Microsoft.EntityFrameworkCore;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    internal class DetailOrderReponsitory : GenericeReponsitory<DetailOrder>, IDetailOrderReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DetailOrderReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateDetailOrder detailorderDTO)
        {
            var de = new DetailOrder
            {
                DetailProductId = detailorderDTO.DetailProductId,
                Quantity = detailorderDTO.Quantity,
                OrderId = detailorderDTO.OrderId
            };
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                _context.DetailOrders.Add(de);
                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                return true;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                throw new Exception("Đã xảy ra lỗi khi thêm chi tiết đơn hàng.", ex);

            }
        }

        /*        public async Task<bool> UpdateDetailOrder(int id, UpdateDetailOrder detailorderDTO)
                {
                    var or = await _context.DetailOrders.FindAsync(id);
                    if (or != null)
                    {
                        or.Quantity = detailorderDTO.Quantity;

                        _context.Orders.Update(or);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }*/
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

        public async Task<ReturnDetailOrder> GetAllAsync()
        {
            var result = new ReturnDetailOrder();

            // Fetch orders including details
            var orders = await _context.DetailOrders// Include DetailOrders for eager loading
                .AsNoTracking()
                .ToListAsync();

            // Map orders to OrderDTO
            result.DetailOrders = _mapper.Map<List<DetailOrderDTO>>(orders);


            return result;
        }
    }
}
