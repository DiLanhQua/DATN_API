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
                _context.Products.Add(product);
                await _context.SaveChangesAsync(); // SaveChanges trước khi tiếp tục

                if (productDTO.ProductDetais?.Count > 0)
                {
                    var productDetails = productDTO.ProductDetais.Select(detail => new DetailProduct
                    {
                        ProductId = product.Id,
                        Size = detail.Size,
                        Price = detail.Price,
                        Quantity = detail.Quantity,
                        ColorId = detail.ColorId,
                        Gender = detail.Gender,
                        Status = detail.Status
                    }).ToList();

                    _context.DetailProducts.AddRange(productDetails);
                }

                if (productDTO.Medias?.Count > 0)
                {
                    foreach (var mediaDto in productDTO.Medias)
                    {
                        var imageLink = await CreateImage(mediaDto.Link);
                        if (!string.IsNullOrEmpty(imageLink))
                        {
                            var image = new Image { Link = imageLink };
                            _context.Images.Add(image);
                            await _context.SaveChangesAsync(); // SaveChanges trước Commit

                            var media = new Media
                            {
                                ProductId = product.Id,
                                IsPrimary = mediaDto.IsPrimary,
                                ImagesId = image.Id
                            };

                            _context.Medium.Add(media);
                        }
                    }
                }

                await _context.SaveChangesAsync(); // SaveChanges cuối cùng
                await transaction.CommitAsync();  // Commit sau cùng
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback nếu có lỗi
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<bool> ChangeStatus(int id)
        {
            try
            {
                Product product = await _context.Products.FindAsync(id);

                product.Status = !product.Status;

                _context.Products.Update(product);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
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
               .Where(m => m.IsPrimary == true && m.ImagesId != null && m.ProductId != null)
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

            List<Product> products = await query
                                    .Skip((brandParams.PageNumber - 1) * brandParams.Pagesize)
                                    .Take(brandParams.Pagesize).ToListAsync();

            List<ProductDEDTO> response = new List<ProductDEDTO>();

            foreach (var p in products)
            {
                DetailProduct detailProduct = await _context.DetailProducts.FirstOrDefaultAsync(a => a.ProductId == p.Id);

                ProductDEDTO productDEDTO = new ProductDEDTO
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    ImagePrimary = primaryImages.ContainsKey(p.Id) ? primaryImages[p.Id] : null,
                    Price = detailProduct.Price | 0,
                    Status = p.Status,
                };

                response.Add(productDEDTO);
            }

            result.Products = response;

            return result;
        }

        public async Task<ProductsUserReturnDtos> GetAllUserAsync(Params brandParams)
        {
            var result = new ProductsUserReturnDtos();

            var query = _context.Products
                        .Where(a => a.Status == true)
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
                .Where(m => m.IsPrimary == true && m.ImagesId != null && m.ProductId != null)
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

            List<Product> products = await query
                        .Skip((brandParams.PageNumber - 1) * brandParams.Pagesize)
                        .Take(brandParams.Pagesize).ToListAsync();

            List<ProductsUserDtos> response = new List<ProductsUserDtos>();

            foreach (var p in products)
            {
                DetailProduct detailProduct = await _context.DetailProducts.FirstOrDefaultAsync(a => a.ProductId == p.Id);

                ProductsUserDtos productDEDTO = new ProductsUserDtos
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    ImagePrimary = primaryImages.ContainsKey(p.Id) ? primaryImages[p.Id] : null,
                    Price = detailProduct.Price | 0,
                    Status = p.Status,
                };

                response.Add(productDEDTO);
            }

            result.Products = response;

            return result;
        }

        public async Task<ProductDEDTO> GetProduct(int id)
        {
            var query = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<ProductDEDTO>(query);
        }

        public async Task<List<ProductsHotSaleDtos>> GetProductsHotSale()
        {
            var detailOrder = await _context.DetailOrders.GroupBy(a => a.DetailProductId)
                               .Select(a => new
                               {
                                   Count = a.Count(),
                                   Id = a.Key,
                               })
                               .OrderByDescending(a => a.Count)
                               .Take(4)
                               .ToListAsync();

            List<ProductsHotSaleDtos> response = new List<ProductsHotSaleDtos>();

            foreach (var item in detailOrder)
            {
                DetailProduct detailProduct = await _context.DetailProducts.FirstOrDefaultAsync(a => a.Id == item.Id);

                Product product = await _context.Products
                                 .Include(a => a.Brand)
                                 .Include(a => a.Category)
                                 .FirstOrDefaultAsync(a => a.Id == detailProduct.ProductId);

                Media media = await _context.Medium.FirstOrDefaultAsync(a => a.ProductId == product.Id && a.IsPrimary == true);

                Image image = await _context.Images.FirstOrDefaultAsync(a => a.Id == media.ImagesId);

                ProductsHotSaleDtos productsItem = new ProductsHotSaleDtos
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Price = detailProduct.Price,
                    BrandId = product.BrandId,
                    BrandName = product.Brand.BrandName,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName,
                    ImagePrimary = image.Link,
                };

                response.Add(productsItem);
            }

            return response;
        }

        public async Task<Media> UpdatePrimaryImage(int idImage)
        {
            try
            {
                Media media = await _context.Medium.FirstOrDefaultAsync(a => a.ImagesId == idImage);

                media.IsPrimary = true;

                _context.Medium.Update(media);

                List<Media> medias = await _context.Medium.Where(a => a.ProductId == media.ProductId && a.ImagesId != idImage).ToListAsync();

                foreach(var item in medias)
                {
                    item.IsPrimary = false;
                    _context.Medium.Update(item);
                }

                await _context.SaveChangesAsync();

                return media;
            }
            catch
            {
                return null;
            }
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
