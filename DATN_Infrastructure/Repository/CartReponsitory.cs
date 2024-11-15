using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class CartReponsitory : GenericeReponsitory<Cart>, ICartReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CartReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreartCart cartDTO)
        {
            var gh = new Cart
            {
                TimeCreated = DateTime.Now,
                AccountId = cartDTO.AccountId,
            };
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                _context.Carts.Add(gh);
                await _context.SaveChangesAsync();

                
                foreach (var product in cartDTO.CartsDTO)
                {
                    var detailCart = new DetailCart
                    {
                        Quantity = product.Quantity,
                        DetailProductId = product.DetailProductId,
                        Carts = gh
                    };

                    _context.DetailCarts.Add(detailCart); // Thêm DetailCart vào context
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                return true;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                throw new Exception("Đã xảy ra lỗi khi thêm tài khoản.", ex);
            
            }
        }

        public async Task<bool> UpdateCart(int id, UpCart cartDTO)
        {
            var gh = await _context.DetailCarts.FindAsync(id);
            if(gh != null)
            {
                gh.Quantity = cartDTO.Quantity;

                _context.DetailCarts.Update(gh);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteCart(int id)
        {
            var gh = await _context.DetailCarts.FindAsync(id);
            if (gh != null)
            {

                _context.DetailCarts.Remove(gh);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ReturnCartDTO> GetAllAsync(Params brandParams)
        {
            var result = new ReturnCartDTO();
            var query = await _context.DetailCarts.Select(p => new CartDe
            {
                DetailProductId = p.DetailProductId,
                Price = p.DetailProduct.Price,
                Quantity = p.Quantity,
                Size = p.DetailProduct.Size,
                Gender = p.DetailProduct.Gender,

            }).AsNoTracking().ToListAsync();
            if (!string.IsNullOrEmpty(brandParams.Search))
            {
                query = query.Where(p => p.DetailProductId.ToString()
                .ToLower().Contains(brandParams.Search)).ToList();
            }
            query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1))
                .Take(brandParams.Pagesize).ToList();
            result.CartsDTO = _mapper.Map<List<CartDe>>(query);
            result.totalItems = query.Count();
            return  result;
        }
    }
}
