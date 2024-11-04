using AutoMapper;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public ICategoryReponsitory CategoryReponsitory { get; }

        public ICartReponsitory CartReponsitory { get; }
        public IBrandReponsitory BrandReponsitory { get; }

        public IProductReponsitory ProductReponsitory { get; }
        public IBlogReponsitory blogReponsitory { get; }
        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            CartReponsitory = new CartReponsitory(_context,  _mapper);
            BrandReponsitory = new BrandReponsitory(_context, _fileProvider, _mapper);
            ProductReponsitory = new ProductReponsitory(_context, _mapper);
            blogReponsitory = new BlogReponsitory(_context, _mapper);
        }
    }
}
