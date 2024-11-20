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
        private readonly IMapper _mapper;
        private readonly EmailDTO _emailDTO;
        private readonly IPasswordHasher<Account> _passwordHasher;
        public ICategoryReponsitory CategoryReponsitory { get; }

        public IEmail EmailReponsitory { get; }
        public ICartReponsitory CartReponsitory { get; }
        public IBrandReponsitory BrandReponsitory { get; }

        public IProductReponsitory ProductReponsitory { get; }
        public IAccountReponsitory AccountReponsitory { get; }
<<<<<<< Updated upstream
=======

        public IColorReponsitory ColorReponsitory { get; }

        public IMediaReponsitory MediaReponsitory { get; }
>>>>>>> Stashed changes
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
            ProductReponsitory = new ProductReponsitory(_context);
            AccountReponsitory = new AccountReponsitory(_context, _mapper, _passwordHasher, _email,_qrCoder);
            EmailReponsitory = new EmailReponsitory(emailDTO);
<<<<<<< Updated upstream
=======
            CartReponsitory = new CartReponsitory(_context, _mapper);
            ColorReponsitory = new ColorRepository(_context, _mapper);
            CategoryReponsitory = new CategoryReponsitory(_context, _fileProvider, _mapper);
            MediaReponsitory = new MediaReponsitory(context, _mapper);

>>>>>>> Stashed changes
        }
    }
}
