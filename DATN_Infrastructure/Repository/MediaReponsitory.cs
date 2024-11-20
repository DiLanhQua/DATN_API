using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
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
        public MediaReponsitory (ApplicationDbContext context, IMapper mapper) : base (context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Media> GetMedia(int idproduct)
        {
            var query = await _context.Medias.FirstOrDefaultAsync(p => p.ProductId == idproduct);

            return _mapper.Map<Media>(query);
        }
    }
}
