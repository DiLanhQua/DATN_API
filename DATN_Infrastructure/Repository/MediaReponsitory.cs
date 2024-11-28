using AutoMapper;
using DATN_Core.Entities;
using DATN_Core.Interface;
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
    public class MediaReponsitory: GenericeReponsitory<Media>, IMediaReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MediaReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Media> GetMedia(int idproduct)
        {
            var query = await _context.Medium.FirstOrDefaultAsync(p => p.ProductId == idproduct);

            return _mapper.Map<Media>(query);
        }
        public async Task<List<Media>> GetALLMedia(int idproduct)
        {
            var query = await _context.Medium.Where(p => p.ProductId == idproduct).ToListAsync();

            return _mapper.Map<List<Media>>(query);
        }
        public async Task<bool> AddMedia(int productId, MediaADD mediaAdd)
        {
            var imageLink = await CreateImage(mediaAdd.Link);
            if (!string.IsNullOrEmpty(imageLink))
            {
                var image = new Image { Link = imageLink };
                _context.Images.AddRange(image);
                await _context.SaveChangesAsync();

                var media = new Media
                {
                    IsPrimary = mediaAdd.IsPrimary,
                    ProductId = productId,
                    ImagesId = image.Id
                };
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Medium.AddRange(media);
                    await _context.SaveChangesAsync();

                    // Log the add action
                    var log = new Login
                    {
                        AccountId = 3, // Example: account that performed the action, change as needed
                        Action = "Thêm media",
                        TimeStamp = DateTime.Now,
                        Description = $"Media" +
                        $" '{media.ProductId}' đã được tạo."
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
            return true;

        }
        public async Task<bool> SetPrimaryImage(int productId, int imageId)
        {
            var allMedia = _context.Medium.Where(m => m.ProductId == productId).ToList();
            foreach (var media in allMedia)
            {
                media.IsPrimary = false; 
            }

            var selectedMedia = allMedia.FirstOrDefault(m => m.ImagesId == imageId);
            if (selectedMedia != null)
            {
                selectedMedia.IsPrimary = true;
                _context.Medium.Update(selectedMedia);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> ChangeImage(int productId, int imageId, MediaADD mediaAdd)
        {
            var imageLink = await CreateImage(mediaAdd.Link);
            if (!string.IsNullOrEmpty(imageLink))
            {
                var currentImage = _context.Images.FirstOrDefault(i => i.Id == imageId);
                if (currentImage != null)
                {
                    currentImage.Link = imageLink;
                    _context.Images.Update(currentImage);
                    await _context.SaveChangesAsync();


                    var media = _context.Medium.FirstOrDefault(m => m.ProductId == productId);
                    if (media != null)
                    {

                        media.IsPrimary = mediaAdd.IsPrimary;
                        _context.Medium.Update(media);
                        await _context.SaveChangesAsync();
                    }
                }

                return true;
            }

            return false;
        }
        public async Task<bool> UpMedia(int productId, MediaADD mediaAdd)
        {
            var imageLink = await CreateImage(mediaAdd.Link);
            if (!string.IsNullOrEmpty(imageLink))
            {
                var image = new Image { Link = imageLink };
                _context.Images.Update(image);
                await _context.SaveChangesAsync();

                var media = new Media
                {
                    IsPrimary = mediaAdd.IsPrimary,
                    ProductId = productId,
                    ImagesId = image.Id
                };
                _context.Medium.Update(media);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> DeleteMedia(int mediaId)
        {
            var media = await _context.Medium.FirstOrDefaultAsync(p => p.Id == mediaId);
            if (media != null)
            {
                //_context.Images.RemoveRange(media.ImagesId);
                _context.Medium.Remove(media);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
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


    }
}
