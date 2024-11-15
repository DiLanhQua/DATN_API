using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
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
        private readonly QrCoder _qrCoder;
        public readonly IEmail _email;
        private readonly EmailDTO _emailDTO;
        private readonly IPasswordHasher<Account> _passwordHasher;

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
        public IEmail EmailReponsitory { get; }
        public IAccountReponsitory AccountReponsitory { get; }

        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper, IOptions<EmailDTO> emailDTO, IEmail email, QrCoder qrCoder, IPasswordHasher<Account> passwordHasher)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            _email = email;
            _emailDTO = emailDTO.Value;
            _qrCoder = qrCoder;
            _passwordHasher = passwordHasher;
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
            AccountReponsitory = new AccountReponsitory(_context, _mapper, _passwordHasher, _email, _qrCoder);
            EmailReponsitory = new EmailReponsitory(emailDTO);
            CartReponsitory = new CartReponsitory(_context, _mapper);

        }
    }
}
