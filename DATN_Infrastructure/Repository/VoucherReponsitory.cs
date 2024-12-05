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
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class VoucherRepository : GenericeReponsitory<Voucher>, IVoucherRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VoucherRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        // Thêm mới Voucher
        public async Task<bool> AddAsync(CreateVoucherDTO voucherDTO)
        {
            var voucher = new Voucher
            {
                VoucherName = voucherDTO.VoucherName,
                TimeStart = voucherDTO.TimeStart,
                TimeEnd = voucherDTO.TimeEnd,
                DiscountType = voucherDTO.DiscountType,
                Quantity = voucherDTO.Quantity,
                Discount = voucherDTO.Discount,
                Min_Order_Value = voucherDTO.Min_Order_Value,
                Max_Discount = voucherDTO.Max_Discount,
                Status = voucherDTO.Status
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Vouchers.AddAsync(voucher);
                await _context.SaveChangesAsync();

                Account admin = _context.Accounts.FirstOrDefault(a => a.Role == 1);

                // Log the add action
                var log = new Login
                {
                    AccountId = admin.Id, // Example: account that performed the action, change as needed
                    Action = "Thêm Voucher",
                    TimeStamp = DateTime.Now,
                    Description = $"Voucher '{voucher.VoucherName}' đã được tạo."
                };

                await _context.Logins.AddAsync(log);
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

        // Cập nhật Voucher
        public async Task<bool> UpdateAsync(int id, UpdateVoucherDTO voucherDTO)
        {
            var currentVoucher = await _context.Vouchers.FindAsync(id);
            if (currentVoucher != null)
            {
                _mapper.Map(voucherDTO, currentVoucher);

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Vouchers.Update(currentVoucher);
                    await _context.SaveChangesAsync();

                    Account admin = _context.Accounts.FirstOrDefault(a => a.Role == 1);

                    // Log the add action
                    var log = new Login
                    {
                        AccountId = admin.Id, // Example: account that performed the action, change as needed
                        Action = "Sửa Voucher",
                        TimeStamp = DateTime.Now,
                        Description = $"Voucher '{voucherDTO.Discount},{voucherDTO.Status},{voucherDTO.Quantity},{voucherDTO.Equals},{voucherDTO.Max_Discount},{voucherDTO.Min_Order_Value},{voucherDTO.TimeEnd},{voucherDTO.TimeStart},{voucherDTO.DiscountType}' đã được sửa."
                    };

                    await _context.Logins.AddAsync(log);
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
            return false;
        }

        // Lấy danh sách Voucher với phân trang và tìm kiếm
        public async Task<ReturnVoucherDTO> GetAllAsync(Params voucherParams)
        {
            var re = new ReturnVoucherDTO();

            var today = DateTime.Today;
            var query = await _context.Vouchers
                .Where(x => x.Quantity != 0 && x.TimeEnd.Date > today)
                .Select(v => new VoucherDTO
                {
                    Id = v.Id,
                    VoucherName = v.VoucherName,
                    TimeStart = v.TimeStart,
                    TimeEnd = v.TimeEnd,
                    DiscountType = v.DiscountType,
                    Quantity = v.Quantity,
                    Discount = v.Discount,
                    Min_Order_Value = v.Min_Order_Value,
                    Max_Discount = v.Max_Discount,
                    Status = v.Status
                })
                .AsNoTracking()
                .ToListAsync();


            // Nếu có tìm kiếm, lọc theo tên voucher
            if (!string.IsNullOrEmpty(voucherParams.Search))
            {
                query = query.Where(v => v.VoucherName.ToLower().Contains(voucherParams.Search.ToLower())).ToList();
            }

            // Phân trang
            query = query
               .Skip(voucherParams.Pagesize * (voucherParams.PageNumber - 1))
               .Take(voucherParams.Pagesize)
               .ToList();

            // Map từ Entity sang DTO
            re.Vouchers = _mapper.Map<List<VoucherDTO>>(query);
            re.TotalItems = query.Count();
            return re;
        }

        public async Task<bool> DeleteQuantilyVoucher(int id)
        {
            var currentVoucher = await _context.Vouchers.FindAsync(id);
            if (currentVoucher != null)
            {
                currentVoucher.Quantity = (byte)(currentVoucher.Quantity - 1);

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Vouchers.Update(currentVoucher);
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
            return false;
        }
    }
}
