using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
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

                    Account admin = _context.Accounts.FirstOrDefault(a => a.Role == 1);

                    var log = new Login
                    {
                        AccountId = admin.Id, // Example: account that performed the action, change as needed
                        Action = "Thêm Product",
                        TimeStamp = DateTime.Now,
                        Description = $"Product '{productDTO.ProductName}' đã được tạo."
                    };

                    await _context.Logins.AddAsync(log);
                  
                    await transaction.CommitAsync();
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

                        var imageLink = await CreateImage(mediaDto.Link); 
                        if (!string.IsNullOrEmpty(imageLink))
                        {
                            var image = new Image { Link = imageLink };
                            _context.Images.Add(image);
                            await _context.SaveChangesAsync(); 

                            var media = new Media
                            {
                                IsPrimary = mediaDto.IsPrimary,
                                BlogId = null,
                                ProductId = productId,
                                ImagesId = image.Id
                            };

                            _context.Medium.Add(media);
                        }  


                        
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
        public async Task<string> CreateImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return null;
            }

            // Xử lý chuỗi base64 và loại bỏ tiền tố "data:image/jpeg;base64," nếu có
            var base64Data = base64String.Split(',')[1];
            var imageBytes = Convert.FromBase64String(base64Data);

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            var fileName = $"{Guid.NewGuid()}.jpg"; // Đặt tên file ngẫu nhiên với định dạng .jpg
            var filePath = Path.Combine(uploadFolder, fileName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // Lưu hình ảnh vào thư mục
            await File.WriteAllBytesAsync(filePath, imageBytes);

            return $"images/products/{fileName}";
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

            var primaryImages = await _context.Medium
               .Where(m => m.IsPrimary == true && m.ImagesId != null)
               .Join(
                   _context.Images,
                   medium => medium.ImagesId.Value, // Đảm bảo ImagesId không null
                   image => image.Id,
                   (medium, image) => new
                   {
                       ProductId = medium.ProductId,
                       ImageLink = image.Link
                   }
               )
               .ToDictionaryAsync(x => x.ProductId, x => x.ImageLink);

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
                    ImagePrimary = primaryImages.ContainsKey(p.Id) ? primaryImages[p.Id] : null

                })
                .ToListAsync();

            return result;
        }

        public async Task<ProductsUserReturnDtos> GetAllUserAsync(Params brandParams)
        {
            var result = new ProductsUserReturnDtos();

            var query = _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Brand)
                        .AsNoTracking()
                        .AsQueryable();

            if (!string.IsNullOrEmpty(brandParams.Search))
            {
                string searchKeyword = brandParams.Search.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(searchKeyword));
            }

            var primaryImages = await _context.Medium
                .Where(m => m.IsPrimary == true && m.ImagesId != null)
                .Join(
                    _context.Images,
                    medium => medium.ImagesId.Value, // Đảm bảo ImagesId không null
                    image => image.Id,
                    (medium, image) => new
                    {
                        ProductId = medium.ProductId,
                        ImageLink = image.Link
                    }
                )
                .ToDictionaryAsync(x => x.ProductId, x => x.ImageLink);

            result.TotalItems = await query.CountAsync();

            result.Products = await query
               .Skip((brandParams.PageNumber - 1) * brandParams.Pagesize)
               .Take(brandParams.Pagesize)
               .Select(p => new ProductsUserDtos
               {
                   Id = p.Id,
                   ProductName = p.ProductName,
                   Description = p.Description,
                   CategoryName = p.Category.CategoryName,
                   BrandName = p.Brand.BrandName,
                   ImagePrimary = primaryImages.ContainsKey(p.Id) ? primaryImages[p.Id] : null
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
        public async Task<bool> UpdateProduct(int id, ProductUPDTO productUP)
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

                product.ProductName = productUP.ProductName;
                product.Description = productUP.Description;
                product.CategoryId = productUP.CategoryId;
                product.BrandId = productUP.BrandId;

                Account admin = _context.Accounts.FirstOrDefault(a => a.Role == 1);

                var log = new Login
                {
                    AccountId = admin.Id, // Example: account that performed the action, change as needed
                    Action = "Sửa Sản Phẩm",
                    TimeStamp = DateTime.Now,
                    Description = $"Sản Phẩm '{productUP.ProductName},{productUP.Description},{productUP.CategoryId},{productUP.BrandId}' đã được sửa."
                };

                await _context.Logins.AddAsync(log);
              
                await transaction.CommitAsync();
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

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
