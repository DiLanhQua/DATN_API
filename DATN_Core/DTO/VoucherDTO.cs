using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class CreateVoucherDTO
    {
        public string VoucherName { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string DiscountType { get; set; }
        public byte Quantity { get; set; }
        public int Discount { get; set; }
        public int Min_Order_Value { get; set; }
        public int Max_Discount { get; set; }
        public byte Status { get; set; }
    }

    // DTO để cập nhật thông tin của Voucher
    public class UpdateVoucherDTO
    {
        public string VoucherName { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string DiscountType { get; set; }
        public byte Quantity { get; set; }
        public int Discount { get; set; }
        public int Min_Order_Value { get; set; }
        public int Max_Discount { get; set; }
        public byte Status { get; set; }
    }

    // DTO dùng để trả về thông tin Voucher cho client
    public class VoucherDTO
    {
        public int Id { get; set; }
        public string VoucherName { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string DiscountType { get; set; }
        public byte Quantity { get; set; }
        public int Discount { get; set; }
        public int Min_Order_Value { get; set; }
        public int Max_Discount { get; set; }
        public byte Status { get; set; }

        // Nếu cần lấy thêm thông tin từ các bảng liên quan, ví dụ: tên sản phẩm, tên tài khoản,...
        // Ví dụ như có thể thêm tên tài khoản tạo voucher (nếu cần thiết)
        public string CreatedByAccountName { get; set; } // Tên tài khoản tạo voucher (nếu có)
    }

    // DTO dùng để trả về danh sách Voucher kèm theo thông tin phân trang
    public class ReturnVoucherDTO
    {
        public int TotalItems { get; set; }  // Tổng số Voucher trong hệ thống
        public List<VoucherDTO> Vouchers { get; set; }  // Danh sách các Voucher DTO
    }
}
