using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace DATN_Infrastructure.Repository
{
    public class LoginReponsitory : GenericeReponsitory<Login>, ILoginReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly IEmail _email;
        private readonly QrCoder _qrCoder;

        public LoginReponsitory(ApplicationDbContext context, IMapper mapper, IPasswordHasher<Account> passwordHasher, IEmail email, QrCoder qrCoder) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _email = email;
            _qrCoder = qrCoder;
        }

        public async Task<ProfileDTO> GetByIdAsync(int id)
        {
            var query = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (query == null)
            {
                return null;
            }
            var result = new ProfileDTO
            {
               
                FullName = query.FullName,
                UserName = query.UserName,
                Email = query.Email,
                Phone = query.Phone,
                Address = query.Address,
                Status = query.Status,
                Password = query.Password,

            };
            return result;
        }

        public async Task<bool> AddAsync(LoginsDTO loginDTO)
        {
            var login = new Login
            {
                AccountId = loginDTO.AccountId,
                Action = loginDTO.Action,
                TimeStamp = DateTime.Now,
                Description = loginDTO.Description
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Logins.AddAsync(login);
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

        public async Task<ReturnLogin> GetAllAsync(Params LoginParams)
        {
            var result = new ReturnLogin();
            var query = await _context.Logins.AsNoTracking().ToListAsync();


            query = query.Skip((LoginParams.Pagesize) * (LoginParams.PageNumber - 1)).Take(LoginParams.Pagesize).ToList();
            result.ListLogins = _mapper.Map<List<ListLoginsDTO>>(query);
            result.totalItems = query.Count;
            return result;
        }

        public async Task<Account> Login(string username, string password)
            {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Tên đăng nhập và mật khẩu không hợp lệ!");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var account = await _context.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserName == username && x.Password == password && x.Role == 2 && x.Status == 1);

                if (account == null)
                    return null;

                

                var loginHistory = new Login
                {
                    AccountId = account.Id,
                    Action = "Đăng nhập",
                    TimeStamp = DateTime.Now.AddMinutes(1),
                    Description = $"Người dùng {account.FullName} đã đăng nhập."
                };

                await _context.Logins.AddAsync(loginHistory);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return account;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


       


       


        //Đăng ký
        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            var register = new Account
            {
                FullName = registerDTO.FullName,
                UserName = registerDTO.UserName,
                Phone = registerDTO.Phone,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                Role = registerDTO.Role,

                Address = registerDTO.Address,
                Image = "Image.png",
            };
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Accounts.Add(register);
                await _context.SaveChangesAsync();
                var login = new Login
                {
                    AccountId = register.Id,
                    Action = "Chờ xác nhận",
                    TimeStamp = DateTime.Now.AddMinutes(1),
                    Description = $"Người dùng {register.FullName} đã đăng ký",
                };
                _context.Logins.Add(login);
                await _context.SaveChangesAsync();

                var maxn = $"https://localhost:7048/api/Login/xn-account/{login.AccountId}";
                var emailBody = new StringBuilder();
                emailBody.AppendLine("Cảm ơn bạn đã đăng ký!");
                emailBody.AppendLine($"<br/><br/><a href='{maxn}' style='padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 15px; height: 40px;'>Xác nhận tài khoản</a>");

                await _email.SendEmail(register.Email, "Xác nhận đăng ký", emailBody.ToString());

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                throw new Exception("Đã xảy ra lỗi khi thêm tài khoản.", ex);
            }
        }

        public async Task<int> XNAccount(int idaccount)
        {
            var tk = await _context.Logins!.FirstOrDefaultAsync(a => a.AccountId == idaccount);
            var account = await _context.Accounts!.FirstOrDefaultAsync(a => a.Id == idaccount);
            if (tk == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }
            if (account == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }


            tk.Action = "Hoạt Động";
            tk.TimeStamp = DateTime.Now;
            tk.Description = "Đã xác nhận tài khoản";

            account.Status = 1;
            _context.Accounts.Update(account);
            _context.Logins.Update(tk);
            await _context.SaveChangesAsync();

            var nv = await _context.Accounts!.FirstOrDefaultAsync(a => a.Id == idaccount);
            var qrCodeContent = $"Tên nhân viên: {nv.FullName}\nSĐT: {nv.Phone}\nEmail: {nv.Email}\nChức vụ: {nv.Role}\nĐịa chỉ: {nv.Address}" +
                   $"\nTên đăng nhập: {nv.UserName}\nMật khẩu: {nv.Password}\nNgày tạo: {tk.TimeStamp}";
            var qrCodeImage = await _qrCoder.QRCodeAsync(qrCodeContent);

            string qrCodeFileName = "qrcode.png";
            var emailBody = new StringBuilder();
            emailBody.AppendLine("Cảm ơn bạn đã đăng ký!");
            emailBody.AppendLine("Vui lòng quét mã QR dưới đây để xác nhận đăng ký của bạn:");
            emailBody.AppendLine($"<img src='cid:{qrCodeFileName}' alt='QR Code' />");

            await _email.SendEmailAsync(nv.Email, "Xác nhận đăng ký", emailBody.ToString(), qrCodeImage, qrCodeFileName);

            return tk.Id;
        }

        public Task<bool> UpdateProfileAsync(int id, UpProfile upprofile)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreartImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }
            var up = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/account");
            var filename = Path.GetFileName(file.FileName);
            var accountName = $"{Guid.NewGuid()}_{filename}";
            var filePath = Path.Combine(up, accountName);
            if (!Directory.Exists(up))
            {
                Directory.CreateDirectory(up);
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"images/account/{accountName}";
        }
    }
}
