using AutoMapper;
<<<<<<< Updated upstream
using DATN_Core.DTO;
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        public MediaReponsitory (ApplicationDbContext context, IMapper mapper) : base (context)
=======
        public MediaReponsitory(ApplicationDbContext context, IMapper mapper) : base(context)
>>>>>>> Stashed changes
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Media> GetMedia(int idproduct)
        {
<<<<<<< Updated upstream
            var query = await _context.Medias.FirstOrDefaultAsync(p => p.ProductId == idproduct);
=======
            var query = await _context.Medium.FirstOrDefaultAsync(p => p.ProductId == idproduct);
>>>>>>> Stashed changes

            return _mapper.Map<Media>(query);
        }
    }
}
