using DATN_Core.Entities;
using DATN_Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DATN_Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using Microsoft.EntityFrameworkCore;
using DATN_Core.DTO;

namespace DATN_Infrastructure.Repository
{
    public class ImagesReponsitory: GenericeReponsitory<Image>, InImageReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public ImagesReponsitory(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateImageDTO imgDTO)
        {
            if (imgDTO.Picture != null)
            {
                var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/brand");
                var sanitizedFileName = imgDTO.Picture.FileName.Replace(" ", "_");
                var imgName = $"{Guid.NewGuid()}_{sanitizedFileName}";

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                var src = Path.Combine(root, imgName);
                using (var fileStream = new FileStream(src, FileMode.Create))
                {
                    await imgDTO.Picture.CopyToAsync(fileStream);
                }

                var res = _mapper.Map<Image>(imgDTO);
                res.Link = $"/images/brand/{imgName}";

                await _context.Images.AddAsync(res);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<ImageDeDTO> GetImage(int idproduct)
        {
            var query = await _context.Images.FirstOrDefaultAsync(p => p.Id == idproduct);
            return _mapper.Map<ImageDeDTO>(query);
        }
        public async Task<ImageDeDTO> GetAllImage(int idproduct)
        {
            var query = await _context.Images.FirstOrDefaultAsync(p => p.Id == idproduct);
            return _mapper.Map<ImageDeDTO>(query);
        }
    }
}
