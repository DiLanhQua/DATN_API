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

        public async Task<bool> AddAsync(CreateProductDTO productDTO)
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

                if (productDTO.ProductDetails != null)
                {
                    foreach (var detailDTO in productDTO.ProductDetails)
                    {
                        var detail = new DetailProduct
                        {
                            Quantity = detailDTO.Quantity,
                            Price = detailDTO.Price,
                            Color = detailDTO.Color,
                            Size = detailDTO.Size,
                            Gender = detailDTO.Gender,
                            Status = detailDTO.Status,
                            Product = product
                        };

                        await _context.DetailProducts.AddAsync(detail);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Đảm bảo rollback
                throw new Exception("Đã xảy ra lỗi khi thêm sản phẩm.", ex);
            }
        }



        public async Task<bool> Updateproduct(int id, UpdateProductDTO proDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the main product and its existing details
                var product = await _context.Products
                    .Include(p => p.DetailProduct)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception($"Product with ID {id} not found.");
                }

                // Update main product properties
                product.ProductName = proDTO.ProductName;
                product.Description = proDTO.Description;
                product.CategoryId = proDTO.CategoryId;
                product.BrandId = proDTO.BrandId;

                // Update or add ProductDetails
                foreach (var detailDTO in proDTO.ProductDetails)
                {
                    var existingDetail = product.DetailProduct
                        .FirstOrDefault(d => d.Id == detailDTO.Id);

                    if (existingDetail != null)
                    {
                        // Update existing detail
                        existingDetail.Size = detailDTO.Size;
                        existingDetail.Price = detailDTO.Price;
                        existingDetail.Quantity = detailDTO.Quantity;
                        existingDetail.Color = detailDTO.Color;
                        existingDetail.Gender = detailDTO.Gender;
                        existingDetail.Status = detailDTO.Status;
                    }
                    else
                    {
                        // Add new detail
                        var newDetail = new DetailProduct
                        {
                            Size = detailDTO.Size,
                            Price = detailDTO.Price,
                            Quantity = detailDTO.Quantity,
                            Color = detailDTO.Color,
                            Gender = detailDTO.Gender,
                            Status = detailDTO.Status,
                            Product = product
                        };
                        _context.DetailProducts.Add(newDetail);
                    }
                }

                // Remove details not included in the update DTO
                var detailIds = proDTO.ProductDetails.Select(d => d.Id).ToList();
                var detailsToRemove = product.DetailProduct
                    .Where(d => !detailIds.Contains(d.Id))
                    .ToList();
                _context.DetailProducts.RemoveRange(detailsToRemove);

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


        public async Task<bool> Deleteproduct(int id)
        {
            var gh = await _context.DetailProducts.FindAsync(id);
            if (gh != null)
            {

                _context.DetailProducts.Remove(gh);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ReturnProductDTO> GetAllAsync(BrandParams brandParams)
        {
            var result = new ReturnProductDTO();

            // Query products with related DetailProduct data included
            var query = _context.Products
                .Include(p => p.DetailProduct)  // Includes product details
                .AsNoTracking()
                .AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(brandParams.Search))
            {
                string searchKeyword = brandParams.Search.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(searchKeyword));
            }

            // Count total items before pagination
            result.TotalItems = await query.CountAsync();

            // Paginate and select products with their details
            result.Products = await query
                .Skip((brandParams.PageNumber - 1) * brandParams.Pagesize)
                .Take(brandParams.Pagesize)
                .Select(p => new ProductDTO
                {
                    ProductName = p.ProductName,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    ProductDetails = p.DetailProduct.Select(pd => new ProductDetailDTO
                    {
                        Id = pd.Id,
                        Size = pd.Size,
                        Price = pd.Price,
                        Quantity = pd.Quantity,
                        Color = pd.Color,
                        Gender = pd.Gender,
                        Status = pd.Status
                    }).ToList()
                })
                .ToListAsync();

            return result;
        }


        //public async Task<ReturnProductDTO> GetAllAsync(BrandParams brandParams)
        //{
        //    var result = new ReturnProductDTO();
        //    var query = await _context.DetailProducts
        //        .Select(p => new ProductDetailDTO
        //        {
        //            Price = p.Price,
        //            Quantity = p.Quantity,
        //            Color = p.Color,
        //            Size = p.Size,
        //            Gender = p.Gender,
        //            Status = p.Status,
        //        }).AsNoTracking().ToListAsync();

        //    if (!string.IsNullOrEmpty(brandParams.Search))
        //    {
        //        query = query.Where(p => p.Id.ToString().ToLower().Contains(brandParams.Search)).ToList();
        //    }

        //    result.TotalItems = query.Count();
        //    query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1)).Take(brandParams.Pagesize).ToList();

        //    result.Products = _mapper.Map<List<ProductDTO>>(query);
        //    return result;
        //}





        //public async Task<ReturnProductDTO> GetAllAsync(BrandParams brandParams)
        //{
        //    var result = new ReturnProductDTO();
        //    var query = _context.Products
        //        .Include(p => p.DetailProduct)
        //        .AsNoTracking()
        //        .AsQueryable();
        //    if (!string.IsNullOrEmpty(brandParams.Search))
        //    {
        //        query = query.Where(p => p.ProductName.ToLower().Contains(brandParams.Search.ToLower()));
        //    }
        //    result.TotalItems = await query.CountAsync();

        //    var products = await query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1))
        //        .Take(brandParams.Pagesize)
        //        .Select(p => new ProductDTO
        //        {
        //            ProductName = p.ProductName,
        //            Description = p.Description,
        //            CategoryId = p.CategoryId,
        //            BrandId = p.BrandId,
        //            ProductDetails = p.DetailProduct.Select(pd => new ProductDetailDTO
        //            {
        //                Id = pd.Id,
        //                Size = pd.Size,
        //                Price = pd.Price,
        //                Quantity = pd.Quantity,
        //                Color = pd.Color,
        //                Gender = pd.Gender,
        //                Status = pd.Status
        //            }).ToList()
        //        })
        //        .ToListAsync();

        //    result.Products = products;
        //    return result;
        //}
    }
}
