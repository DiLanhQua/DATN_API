using AutoMapper;
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
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class CategoryReponsitory : GenericeReponsitory<Category>, ICategoryReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public CategoryReponsitory(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateCatagoryDTO CatagoryDTO)
        {
            if (CatagoryDTO.Picture != null)
            {
                var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/brand");
                var sanitizedFileName = CatagoryDTO.Picture.FileName.Replace(" ", "_");
                var cataName = $"{Guid.NewGuid()}_{sanitizedFileName}";

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                var src = Path.Combine(root, cataName);
                using (var fileStream = new FileStream(src, FileMode.Create))
                {
                    await CatagoryDTO.Picture.CopyToAsync(fileStream);
                }

                var res = _mapper.Map<Category>(CatagoryDTO);
                res.Image = $"/images/brand/{cataName}";

                
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Categories.AddAsync(res);
                    await _context.SaveChangesAsync();

                    // Log the add action
                    var log = new Login
                    {
                        AccountId = 100, // Example: account that performed the action, change as needed
                        Action = "Thêm Category",
                        TimeStamp = DateTime.Now,
                        Description = $"Category '{CatagoryDTO.CategoryName}' đã được tạo."
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

        public async Task<ReturnCatagoryDTO> GetAllAsync(Params brandParams)
        {
            var result = new ReturnCatagoryDTO();
            var query = await _context.Categories.AsNoTracking().ToListAsync();
            if (!string.IsNullOrEmpty(brandParams.Search))
                query = query.Where(x => x.CategoryName.ToLower().Contains(brandParams.Search)).ToList();
            query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1)).Take(brandParams.Pagesize).ToList();
            result.cataDTOs = _mapper.Map<List<CataDTO>>(query);
            result.totalItems = query.Count;
            return result;


        }

        public async Task<bool> UpdateAsync(int id, UpdateCatagoryDTO catagoryDTO)
        {
            // Tìm đối tượng hiện tại trong database
            var currentBrand = await _context.Categories.FindAsync(id);
            if (currentBrand != null)
            {
                // Kiểm tra nếu có ảnh mới được tải lên
                if (catagoryDTO.Picture != null)
                {
                    // Đường dẫn lưu ảnh
                    var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/brand");
                    var sanitizedFileName = catagoryDTO.Picture.FileName.Replace(" ", "_");
                    var cataName = $"{Guid.NewGuid()}_{sanitizedFileName}";

                    // Kiểm tra và tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }

                    // Đường dẫn ảnh đầy đủ
                    var src = Path.Combine(root, cataName);
                    using (var fileStream = new FileStream(src, FileMode.Create))
                    {
                        await catagoryDTO.Picture.CopyToAsync(fileStream);
                    }

                    // Kiểm tra và xóa ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(currentBrand.Image))
                    {
                        var picInfo = _fileProvider.GetFileInfo(currentBrand.Image);
                        if (picInfo.Exists)
                        {
                            var rootPath = picInfo.PhysicalPath;
                            System.IO.File.Delete(rootPath);
                        }
                    }

                    // Cập nhật đường dẫn ảnh mới dưới dạng đường dẫn tương đối
                    currentBrand.Image = $"/images/brand/{cataName}";
                }

                // Cập nhật các thuộc tính khác từ brandDTO
                _mapper.Map(catagoryDTO, currentBrand);
                
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Categories.Update(currentBrand);
                    await _context.SaveChangesAsync();

                    // Log the add action
                    var log = new Login
                    {
                        AccountId = 100, // Example: account that performed the action, change as needed
                        Action = "Sửa Category",
                        TimeStamp = DateTime.Now,
                        Description = $"Category '{catagoryDTO.CategoryName},{catagoryDTO.oldImage},{catagoryDTO.Picture}' đã được sửa."
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
    }
}
