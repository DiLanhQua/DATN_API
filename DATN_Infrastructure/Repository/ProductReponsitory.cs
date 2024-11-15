using AutoMapper;
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
    public class ProductReponsitory : GenericeReponsitory<Product>, IProductReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(ProductDTO productDTO)
        {
            var product = new Product
            {
                ProductName = productDTO.ProductName,
                Description = productDTO.Description,
                BrandId = productDTO.BrandId,
                CategoryId = productDTO.CategoryId,
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();


            

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); 
                throw new Exception("Đã xảy ra lỗi khi thêm sản phẩm.", ex);
            }
        }



      

        public async Task<bool> Deleteproduct(int id)
        {
            var gh = await _context.Products.FindAsync(id);
            if (gh != null)
            {

                _context.Products.Remove(gh);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ReturnProductDTO> GetAllAsync(Params brandParams)
        {
            var result = new ReturnProductDTO();

            // Query products with related DetailProduct data included
            var query = _context.Products
               
                .AsNoTracking()
                .AsQueryable();

            
            if (!string.IsNullOrEmpty(brandParams.Search))
            {
                string searchKeyword = brandParams.Search.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(searchKeyword));
            }

           
            result.TotalItems = await query.CountAsync();

           
            result.Products = await query
                .Skip((brandParams.PageNumber - 1) * brandParams.Pagesize)
                .Take(brandParams.Pagesize)
                .Select(p => new ProductDTO
                {
                    ProductName = p.ProductName,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                  
                })
                .ToListAsync();

            return result;
        }

        public async Task<bool> Updateproduct(int id, ProductDTO ProDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the main product and its existing details
                var product = await _context.Products                  
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception($"Product with ID {id} not found.");
                }

                // Update main product properties
                product.ProductName = ProDTO.ProductName;
                product.Description = ProDTO.Description;
                product.CategoryId = ProDTO.CategoryId;
                product.BrandId = ProDTO.BrandId;

                

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
