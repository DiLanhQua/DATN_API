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
        public IBrandReponsitory BrandReponsitory { get; }

        public ICartReponsitory CartReponsitory { get; }
        public IProductReponsitory ProductReponsitory { get; }

        public InImageReponsitory InImageReponsitory { get; }
        public IOrderReponsitory OrderReponsitory { get; }
        public IDetailOrderReponsitory DetailOrderReponsitory { get; }
        public IDetailProductReponsitory DetailProductReponsitory { get; }
        public IBlogReponsitory BlogReponsitory { get; }
        public IVoucherRepository VoucherRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            CartReponsitory = new CartReponsitory(_context,  _mapper);
            BrandReponsitory = new BrandReponsitory(_context, _fileProvider, _mapper);
            InImageReponsitory = new ImagesReponsitory(_context, _fileProvider, _mapper);
            OrderReponsitory = new OrderReponsitory(_context, _mapper);
            DetailOrderReponsitory = new DetailOrderReponsitory(_context, _mapper);
            ProductReponsitory = new ProductReponsitory(_context, _mapper);
            DetailProductReponsitory = new DetailProductReponsitory(_context, _mapper);
            BlogReponsitory = new BlogReponsitory(_context, _mapper);
            CommentRepository = new CommentRepository(_context, _mapper);
            VoucherRepository = new VoucherRepository(_context, _mapper);
        }
    }
}
