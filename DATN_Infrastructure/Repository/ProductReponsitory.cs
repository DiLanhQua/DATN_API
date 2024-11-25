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
    public class ProductReponsitory : GenericeReponsitory<Product>, IProductReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddProduct(ProductDTO productDTO)
        {
            // Tạo đối tượng sản phẩm
            var product = new Product
            {
                ProductName = productDTO.ProductName,
                Description = productDTO.Description,
                BrandId = productDTO.BrandId,
                CategoryId = productDTO.CategoryId,
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Thêm sản phẩm vào cơ sở dữ liệu
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                int productId = product.Id;


                if (productDTO.ProductDetais != null && productDTO.ProductDetais.Count > 0 )
                {
                    var productDetails = productDTO.ProductDetais.Select(detail => new DetailProduct
                    {
                        Size = detail.Size,
                        Price = detail.Price,
                        Quantity = detail.Quantity,
                        ColorId = detail.ColorId,
                        Gender = detail.Gender,
                        Status = detail.Status,
                        ProductId = productId
                    }).ToList();

                    _context.DetailProducts.AddRange(productDetails);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("Không có chi tiết sản phẩm để thêm.");
                }

                // Thêm media nếu có
                if (productDTO.Medias != null && productDTO.Medias.Count > 0)
                {
                    foreach (var mediaDto in productDTO.Medias)
                    {
                        
                            var image = new Image { Link = mediaDto.Link };
                            _context.Images.Add(image);
                            await _context.SaveChangesAsync();  // Save the image to get image.Id


                        // Thêm media liên kết với sản phẩm
                        var media = new Media
                        {
                            IsPrimary = true,
                            BlogId = 1,
                            ProductId = productId,
                            ImageId = image.Id
                        };

                        _context.Medium.Add(media);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Lỗi khi thêm sản phẩm: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Chi tiết lỗi: {ex.InnerException.Message}");
                }
                throw;
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
                .Select(p => new ProductDEDTO
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,

                })
                .ToListAsync();

            return result;
        }

        public async Task<ProductDEDTO> GetProduct(int id)
        {
            var query = await _context.Products
        .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<ProductDEDTO>(query);
        }
        public async Task<bool> UpdateProduct(int id, ProductDTO productDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception($"Product with ID {id} not found.");
                }

                // Cập nhật thuộc tính sản phẩm
                product.ProductName = productDTO.ProductName;
                product.Description = productDTO.Description;
                product.CategoryId = productDTO.CategoryId;
                product.BrandId = productDTO.BrandId;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                foreach (var detailDTO in productDTO.ProductDetais)
                {
                    var existingDetail = await _context.DetailProducts
                        .FirstOrDefaultAsync(p => p.ProductId == id );

                    if (existingDetail != null)
                    {
                        existingDetail.Size = detailDTO.Size;
                        existingDetail.Price = detailDTO.Price;
                        existingDetail.Quantity = detailDTO.Quantity;
                        existingDetail.ColorId = detailDTO.ColorId;
                        existingDetail.Gender = detailDTO.Gender;
                        existingDetail.Status = detailDTO.Status;

                        _context.DetailProducts.Update(existingDetail);
                    }
                    else
                    {
                        var newDetail = productDTO.ProductDetais.Select(detail => new DetailProduct
                        {
                            Size = detail.Size,
                            Price = detail.Price,
                            Quantity = detail.Quantity,
                            ColorId = detail.ColorId,
                            Gender = detail.Gender,
                            Status = detail.Status,
                            ProductId = id
                        }).ToList();
                        _context.DetailProducts.AddRange(newDetail);
                    }

                }

                // Cập nhật media
                foreach (var mediaDto in productDTO.Medias)
                {
                    var existingMedia = await _context.Medium
                        .FirstOrDefaultAsync(em => em.ProductId == id);
                    var existingimgae = await _context.Images
                        .FirstOrDefaultAsync(em => em.Id == existingMedia.ImageId);
                    if (existingMedia != null)
                    {
                        existingimgae.Link = mediaDto.Link;
                        existingMedia.IsPrimary = mediaDto.IsPrimary;

                        _context.Images.Update(existingimgae);

                        _context.Medium.Update(existingMedia);
                    }
                    else
                    {
                        var newImage = new Image { Link = mediaDto.Link };
                        _context.Images.Add(newImage);
                        await _context.SaveChangesAsync();

                        var newMedia = new Media
                        {
                            IsPrimary = mediaDto.IsPrimary,
                            BlogId = 1, // assuming this is correct and needed
                            ProductId = id,
                            ImageId = newImage.Id
                        };
                        _context.Medium.Add(newMedia);

                        await _context.SaveChangesAsync();
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }
}
