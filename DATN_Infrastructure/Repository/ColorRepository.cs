using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DATN_Infrastructure.Repository
{
    public class ColorRepository : GenericeReponsitory<Color>, IColorReponsitory
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ColorRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddColor(ColorDTO colordto)
        {
            var Createcolor = new Color
            {
                NameColor = colordto.NameColor,
                ColorCode = colordto.ColorCode
            };

            using var Transaction = await _context.Database.BeginTransactionAsync();
            try
            {
               
                await _context.Colors.AddAsync(Createcolor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Transaction.RollbackAsync();
                throw new Exception("Đã xảy ra lỗi khi thêm màu.", ex);
            }
        }

        public async Task<bool> DeleteColor(int id)
        {
            var query = await _context.Colors.FindAsync(id);
            if(query != null)
            {
                _context.Colors.Remove(query);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Color> GetColor(int idcolor)
        {
            var nv = await _context.Colors!.FirstOrDefaultAsync(a => a.Id == idcolor);
            return _mapper.Map<Color>(nv);
        }

        public Task<ReturnColorDTO> GetAllAsync(Params colorParams)
        {
            var result = new ReturnColorDTO();
            var query = _context.Colors.AsNoTracking().ToList();
            query = query.Skip((colorParams.Pagesize) * (colorParams.PageNumber - 1)).Take(colorParams.Pagesize).ToList();
            result.ColorList = _mapper.Map<List<ColorDTO>>(query);
            result.TotalItems = query.Count;
            return Task.FromResult(result);
        }

        public async Task<bool> UpdateColor(int id, ColorDTO colordto)
        {
            var exittingColor = await _context.Colors.FindAsync(id);
            if (exittingColor != null)
            {
                exittingColor.NameColor = colordto.NameColor;
                exittingColor.ColorCode = colordto.ColorCode;
                await _context.SaveChangesAsync();
                return true;
            }
            throw new Exception($"Không tìm thấy mã màu {id}");
            
        }
    }
}
