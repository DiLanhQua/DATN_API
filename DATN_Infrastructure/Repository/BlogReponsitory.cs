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
    public class BlogReponsitory : GenericeReponsitory<Blog>, IBlogReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BlogReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddAsync(CreateBlogDTO blogDTO)
        {
            if (blogDTO.HeadLine != null)
            {
                Blog mediaEntity = new Blog
                {
                    AccountId = blogDTO.AccountId,
                    Content = blogDTO.Content,
                    DatePush = DateTime.Now,
                    HeadLine = blogDTO.HeadLine,
                };
               
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    await _context.Blogs.AddAsync(mediaEntity);
                    await _context.SaveChangesAsync();

                    // Log the add action
                    Login log = new Login
                    {
                        AccountId = blogDTO.AccountId, // Example: account that performed the action, change as needed
                        Action = "Thêm Blog",
                        TimeStamp = DateTime.Now,
                        Description = $"Blog '{blogDTO.HeadLine}' đã được tạo."
                    };

                    await _context.Logins.AddAsync(log);
                    await _context.SaveChangesAsync();

                    if (blogDTO.Images.Count > 0)
                    {
                        foreach (var mediaDto in blogDTO.Images)
                        {
                            string imageLink = await CreateImage(mediaDto.Url);

                            if (!string.IsNullOrEmpty(imageLink))
                            {
                                Image image = new Image { Link = imageLink };
                                
                                _context.Images.Add(image);

                                await _context.SaveChangesAsync(); // SaveChanges trước Commit

                                Media media = new Media
                                {
                                    BlogId = mediaEntity.Id,
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
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            return false;
        }

        private async Task<string> CreateImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return null;
            }

            // Xử lý chuỗi base64 và loại bỏ tiền tố "data:image/jpeg;base64," nếu có
            var base64Data = base64String.Split(',')[1];
            var imageBytes = Convert.FromBase64String(base64Data);

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog");
            var fileName = $"{Guid.NewGuid()}.jpg"; // Đặt tên file ngẫu nhiên với định dạng .jpg
            var filePath = Path.Combine(uploadFolder, fileName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // Lưu hình ảnh vào thư mục
            await File.WriteAllBytesAsync(filePath, imageBytes);

            return $"images/blog/{fileName}";
        }


        public async Task<ReturnBlogDTO> GetAllAsync(Params brandParams)
        {
            var result = new ReturnBlogDTO();
            
            List<Blog> query = await _context.Blogs.Include(a => a.Account).AsNoTracking().ToListAsync();
            
            query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1)).Take(brandParams.Pagesize).ToList();

            List<BlogDTO> res = new List<BlogDTO>();

            foreach (var blog in query)
            {
                Media media = await _context.Medium.FirstOrDefaultAsync(a => a.BlogId == blog.Id && a.IsPrimary == true);

                Image image = await _context.Images.FirstOrDefaultAsync(a => a.Id == media.ImagesId);

                BlogDTO item = new BlogDTO
                {
                    Id = blog.Id,
                    HeadLine = blog.HeadLine,
                    DatePush = blog.DatePush,
                    Content = blog.Content,
                    AccountId = blog.AccountId,
                    FullName = blog.Account.FullName,
                    Image = image.Link,
                };

                res.Add(item);
            }
            
            result.BlogsDTO = res;
            
            result.totalItems = query.Count;
            
            return result;
        }

        public async Task<bool> UpdateAsync(int id, CreateBlogDTO BlogDTO)
        {

            var currentBrand = await _context.Blogs.FindAsync(id);

            if (currentBrand != null)
            {
                _mapper.Map(BlogDTO, currentBrand);
               
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Blogs.Update(currentBrand);
                   await _context.SaveChangesAsync();

                   if(BlogDTO.Images.Count > 0)
                   {
                       await CreateMedia(BlogDTO.Images, id);
                   }

                   // Log the add action
                   var log = new Login
                    {
                        AccountId = BlogDTO.AccountId, // Example: account that performed the action, change as needed
                        Action = "Update Blog",
                        TimeStamp = DateTime.Now,
                        Description = $"Update Blog '{BlogDTO.HeadLine}' đã được tạo."
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

            return false;
        }

        private async Task CreateMedia(List<ImageBlog> data, int idBlog)
        {
            foreach (var mediaDto in data)
            {
                string imageLink = await CreateImage(mediaDto.Url);

                if (!string.IsNullOrEmpty(imageLink))
                {
                    Image image = new Image { Link = imageLink };

                    _context.Images.Add(image);

                    await _context.SaveChangesAsync(); // SaveChanges trước Commit

                    Media media = new Media
                    {
                        BlogId = idBlog,
                        IsPrimary = false,
                        ImagesId = image.Id
                    };

                    _context.Medium.Add(media);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<BlogDTO> GetBlogByIdAsync(int id)
        {
            Blog blog = await _context.Blogs.FindAsync(id);

            return _mapper.Map<BlogDTO>(blog);
        }

        public async Task<List<ImageBlogDtos>> GetImageByIdBlog(int idBlog)
        {
            List<Media> media = await _context.Medium.Where(a => a.BlogId == idBlog).ToListAsync();

            List<ImageBlogDtos> response = new List<ImageBlogDtos>();

            foreach (var mediaItem in media)
            {
                Image image = await _context.Images.FirstOrDefaultAsync(a => a.Id == mediaItem.ImagesId);

                ImageBlogDtos item = new ImageBlogDtos
                {
                    Id = mediaItem.Id,
                    IsPrimary = mediaItem.IsPrimary,
                    Link = image.Link,
                };
                response.Add(item);
            }

            return response;
        }

        public async Task<bool> DeleteImageBlog(int idImage)
        {
            try
            {
                Media media = await _context.Medium.FirstOrDefaultAsync(a => a.ImagesId == idImage);

                Image image = await _context.Images.FindAsync(idImage);

                _context.Medium.Remove(media);

                _context.Images.Remove(image);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsPrimaryBlog(int idImage)
        {
            try
            {
                Media media = await _context.Medium.FirstOrDefaultAsync(a => a.ImagesId == idImage);

                List<Media> mediaList = await _context.Medium.Where(a => a.BlogId == media.BlogId && a.ImagesId != idImage).ToListAsync();

                foreach (Media mediaItem in mediaList)
                {
                    mediaItem.IsPrimary = false;

                    _context.Medium.Update(mediaItem);
                }

                media.IsPrimary = true;

                _context.Medium.Update(media);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBlogById(int id)
        {
            try
            {
                Blog blog = await _context.Blogs.FindAsync(id);

                List<Media> medias = await _context.Medium.Where(a => a.BlogId == id).ToListAsync();

                foreach (Media media in medias)
                {
                    Image image = await _context.Images.FindAsync(media.ImagesId);
                    
                    _context.Medium.Remove(media);

                    _context.Images.Remove(image);
                }

                _context.Blogs.Remove(blog);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
