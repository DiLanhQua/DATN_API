using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Sharing;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    // Thay đổi từ ICommentRepository thành IVoucherRepository và thay đổi các loại dữ liệu tương ứng
    public interface IVoucherRepository : IGenericeReponsitory<Voucher>
    {
        // Phương thức trả về danh sách Voucher với phân trang và tìm kiếm
        Task<ReturnVoucherDTO> GetAllAsync(Params voucherParams);

        // Phương thức thêm mới Voucher
        Task<bool> AddAsync(CreateVoucherDTO voucherDTO);

        // Phương thức cập nhật Voucher
        Task<bool> UpdateAsync(int id, UpdateVoucherDTO voucherDTO);
        Task<bool> DeleteQuantilyVoucher(int id);
    }
}
