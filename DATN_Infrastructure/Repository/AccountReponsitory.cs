using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class AccountReponsitory : GenericeReponsitory<Account>, IAccountReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly IEmail _email;
        private readonly QrCoder _qrCoder;
        public AccountReponsitory(ApplicationDbContext context, IMapper mapper,
            IPasswordHasher<Account> passwordHasher, IEmail email, QrCoder qrCoder ) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _email = email;
            _qrCoder = qrCoder;
        }

        public async Task<bool> AddAccount(CreartAccount accountDTO)
        {
            var nv = new Account
            {
                FullName = accountDTO.FullName,
                UserName = accountDTO.UserName,
                Phone = accountDTO.Phone,
                Email = accountDTO.Email,
                Password =  accountDTO.Password,
                Role = accountDTO.Role,
                Address = accountDTO.Address,
                Image = await CreartImage(accountDTO.Picture),
            };
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Accounts.Add(nv);
                await _context.SaveChangesAsync();
                var login = new Login
                {
                    AccountId = nv.Id,
                    Action = "Chờ xác nhận",
                    TimeStamp = DateTime.Now.AddMinutes(1),
                    Description = "",
                };
                _context.Logins.Add(login);
                await _context.SaveChangesAsync();

                var maxn = $"https://localhost:7048/api/Account/xn-account/{login.AccountId}";
                var emailBody = new StringBuilder();
                emailBody.AppendLine("Cảm ơn bạn đã đăng ký!");
                emailBody.AppendLine($"<br/><br/><a href=\"{maxn}\" style=\"padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none;\">Xác nhận tài khoản</a>");
                await _email.SendEmail(nv.Email, "Xác nhận đăng ký", emailBody.ToString());

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                throw new Exception("Đã xảy ra lỗi khi thêm tài khoản.", ex);
            }
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

        public async Task<ReturnAccountDTO> GetAllAsync(Params brandParams)
        {
            var result = new ReturnAccountDTO();
            var query = await _context.Accounts!.AsNoTracking().ToListAsync();
            if (!string.IsNullOrEmpty(brandParams.Search))
            {
                query = query.Where(p => p.FullName.ToString()
                .ToLower().Contains(brandParams.Search)).ToList();
            }
            query = query.Skip((brandParams.Pagesize) * (brandParams.PageNumber - 1))
                .Take(brandParams.Pagesize).ToList();
            result.AccountsDTO = _mapper.Map<List<AccountDTO>>(query);
            result.totalItems = query.Count();
            return result;
        }

        public async Task<LoginCT> GetDeLogin(int idaccount)
        {
            var nv = await _context.Logins!.FirstOrDefaultAsync(a => a.AccountId == idaccount);
            return _mapper.Map<LoginCT>(nv);
        }
        public async Task<AccountDTO> GetDeAccount(int idaccount)
        {
            var nv = await _context.Accounts!.FirstOrDefaultAsync(a => a.Id == idaccount);
            return _mapper.Map<AccountDTO>(nv);
        }

        public async Task<Account> Login(string username, string password)
        {
            // Truy vấn cơ sở dữ liệu để kiểm tra tài khoản
            return await _context.Accounts!
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == username && x.Password == password && x.Role == 1);
        }




        public async Task<bool> UpAccount(int idaccount, UpAccount upAccount)
        {
            if (idaccount != upAccount.AccountId)
            {
                throw new Exception("ID nhân viên không khớp.");
            }
            try
            {
                var ex = await _context.Accounts!.FindAsync(idaccount);
                if (ex == null)
                {
                    throw new Exception("Nhân viên không tồn tại");
                }

                ex.FullName = upAccount.FullName;
                ex.Phone = upAccount.Phone;
                ex.Email = upAccount.Email;
                ex.Password = _passwordHasher.HashPassword(null, upAccount.Password); // Bảo mật mật khẩu
                ex.UserName = upAccount.UserName;
                ex.Address = upAccount.Address;
                ex.Role = upAccount.Role;
                ex.Image = await CreartImage(upAccount.Picture);

                _context.Accounts.Update(ex);
                await _context.SaveChangesAsync();

                var qrCodeContent = $"Tên nhân viên: {ex.FullName}\nSĐT: {ex.Phone}\nEmail: {ex.Email}\nChức vụ: {ex.Role}\nĐịa chỉ: {ex.Address}\nTên đăng nhập: {ex.UserName}\nMật khẩu: {ex.Password}";
                var qrCodeImage = await _qrCoder.QRCodeAsync(qrCodeContent);

                string qrCodeFileName = "qrcode.png";

                var emailBody = new StringBuilder();
                emailBody.AppendLine("Tài khoản đã được thay đổi!");
                emailBody.AppendLine("Vui lòng quét mã QR dưới đây để xác nhận tài khoản đã được thay đổi của bạn:");
                emailBody.AppendLine($"<img src='cid:{qrCodeFileName}' alt='QR Code' />");

                await _email.SendEmailAsync(ex.Email, "Xác nhận tài khoản đã được thay đổi", emailBody.ToString(), qrCodeImage, qrCodeFileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Đã xảy ra lỗi khi cập nhật tài khoản.", ex);
            }
        }

        public async Task<int> XNAccount(int idaccount)
        {
           var tk = await _context.Logins!.FirstOrDefaultAsync(a => a.AccountId == idaccount);
            var ac = await _context.Accounts!.FirstOrDefaultAsync(a => a.Id == idaccount);
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
            ac.Status = 1;
            tk.Description = "Đã xác nhận tài khoản";

            account.Status = 1;
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

        public async Task<bool> BlockAccount(int id)
        {
            try
            {
                // Lấy tài khoản từ cơ sở dữ liệu theo AccountId
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
                if (account == null)
                {
                    throw new Exception("Tài khoản không tồn tại.");
                }

                // Cập nhật trạng thái tài khoản từ giá trị Status trong DTO
                account.Status = 2;  // Cập nhật trạng thái tài khoản

                // Cập nhật vào cơ sở dữ liệu
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi cấm tài khoản.", ex);
            }
        }
        public async Task<bool> UnBlockAccount(int id)
        {
            try
            {
                // Lấy tài khoản từ cơ sở dữ liệu theo AccountId
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
                if (account == null)
                {
                    throw new Exception("Tài khoản không tồn tại.");
                }

                // Cập nhật trạng thái tài khoản từ giá trị Status trong DTO
                account.Status = 1;  // Cập nhật trạng thái tài khoản

                // Cập nhật vào cơ sở dữ liệu
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi un lock tài khoản.", ex);
            }
        }
    }
}
