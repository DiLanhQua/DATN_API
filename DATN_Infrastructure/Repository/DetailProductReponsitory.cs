using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    internal class DetailProductReponsitory : GenericeReponsitory<DetailProduct>, IDetailProductReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DetailProductReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(ProductDetailDTO ProDTO)
        {
            var Productdetail = new DetailProduct
            {
                Size = ProDTO.Size,
                Price = ProDTO.Price,
                Quantity = ProDTO.Quantity,
                Color = ProDTO.Color,
                Gender = ProDTO.Gender,
                Status = ProDTO.Status,
                ProductId = ProDTO.ProductId
            };

            using var consaction = await  _context.Database.BeginTransactionAsync();

            try
            {
                await _context.DetailProducts.AddAsync(Productdetail);
                await _context.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {

                await consaction.RollbackAsync();
                throw new Exception("Đã xảy ra lỗi khi thêm sản phẩm.", ex);
            }

           
        

       
    
        }

        public async Task<bool> Deleteproduct(int id)
        {
            var query = await _context.DetailProducts.FindAsync(id);

            if(query != null)
            {
                _context.DetailProducts.Remove(query);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
            
        }

        public async Task<ReturnProductDetailDTO> GetAllAsync(BrandParams brandParams)
        {
            var result = new ReturnProductDetailDTO();
            var query = await _context.DetailProducts.AsNoTracking().ToListAsync();

          
            query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1)).Take(brandParams.Pagesize).ToList();
            result.Productdetail = _mapper.Map<List<ProductDetailDTO>>(query);
            result.TotalItems = query.Count;
            return result;

        }



        public async Task<bool> UpdateAsync(int id, ProductDetailDTO ProDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var exiting = await _context.DetailProducts.FirstOrDefaultAsync(x => x.Id == id);

                if (exiting == null)
                {
                    throw new Exception("Không tìm thấy sản phẩm");
                }
                exiting.Size = ProDTO.Size;
                exiting.Price = ProDTO.Price;
                exiting.Quantity = ProDTO.Quantity;
                exiting.Color = ProDTO.Color;
                exiting.Gender = ProDTO.Gender;
                exiting.Status = ProDTO.Status;
                exiting.ProductId = ProDTO.ProductId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("An error occurred while updating the product.", ex);
            }
            
    
        }
    }
}
