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

            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

        // Cập nhật Voucher
        public async Task<bool> UpdateAsync(int id, UpdateVoucherDTO voucherDTO)
        {
            var currentVoucher = await _context.Vouchers.FindAsync(id);
            if (currentVoucher != null)
            {
                _mapper.Map(voucherDTO, currentVoucher);
                _context.Vouchers.Update(currentVoucher);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Lấy danh sách Voucher với phân trang và tìm kiếm
        public async Task<ReturnVoucherDTO> GetAllAsync(Params voucherParams)
        {
            var re = new ReturnVoucherDTO();

            var query = await _context.Vouchers.Select(v => new VoucherDTO
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
            }).AsNoTracking().ToListAsync();

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
    }
}
