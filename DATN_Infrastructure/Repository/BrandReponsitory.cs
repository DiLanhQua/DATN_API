using AutoMapper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class BrandReponsitory : GenericeReponsitory<Brand>, IBrandReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public BrandReponsitory(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateBrandDTO brandDTO)
        {
            if (brandDTO.Picture != null)
            {
                var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/brand");
                var sanitizedFileName = brandDTO.Picture.FileName.Replace(" ", "_");
                var brandName = $"{Guid.NewGuid()}_{sanitizedFileName}";

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                var src = Path.Combine(root, brandName);
                using (var fileStream = new FileStream(src, FileMode.Create))
                {
                    await brandDTO.Picture.CopyToAsync(fileStream);
                }

                var res = _mapper.Map<Brand>(brandDTO);
                res.Image = $"/images/brand/{brandName}";

                await _context.Brands.AddAsync(res);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ReturnBrandDTO> GetAllAsync(BrandParams brandParams)
        {
            var result = new ReturnBrandDTO();
            var query = await _context.Brands.AsNoTracking().ToListAsync();
            if (!string.IsNullOrEmpty(brandParams.Search))
                query = query.Where(x => x.BrandName.ToLower().Contains(brandParams.Search)).ToList();
            query = query.Skip((brandParams.Pagesize)*(brandParams.PageNumber-1)).Take(brandParams.Pagesize).ToList();
            result.BrandsDTO = _mapper.Map<List<BrandDTO>>(query);
            result.totalItems = query.Count;
            return result;  
           

        }

        public async Task<bool> UpdateAsync(int id, UpdateBrandDTO brandDTO)
        {
            // Tìm đối tượng hiện tại trong database
            var currentBrand = await _context.Brands.FindAsync(id);
            if (currentBrand != null)
            {
                // Kiểm tra nếu có ảnh mới được tải lên
                if (brandDTO.Picture != null)
                {
                    // Đường dẫn lưu ảnh
                    var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/brand");
                    var sanitizedFileName = brandDTO.Picture.FileName.Replace(" ", "_");
                    var brandName = $"{Guid.NewGuid()}_{sanitizedFileName}";

                    // Kiểm tra và tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }

                    // Đường dẫn ảnh đầy đủ
                    var src = Path.Combine(root, brandName);
                    using (var fileStream = new FileStream(src, FileMode.Create))
                    {
                        await brandDTO.Picture.CopyToAsync(fileStream);
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
                    currentBrand.Image = $"/images/brand/{brandName}";
                }

                // Cập nhật các thuộc tính khác từ brandDTO
                _mapper.Map(brandDTO, currentBrand);
                _context.Brands.Update(currentBrand);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
