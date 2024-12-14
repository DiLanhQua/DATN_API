using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DATN_Core.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmail _emailService;
        private readonly IUnitOfWork _uow;


        public EmailController(IEmail emailService, IUnitOfWork uow)
        {
            _emailService = emailService;
            _uow = uow;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            var account = _uow.AccountReponsitory.GetByIdAsync(request.accountId);

            if (request == null && account ==null)
                return BadRequest("Invalid email request.");

            try
            {
                // Đường dẫn tới thư mục wwwroot
                var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-templates", "template.html");

                // Kiểm tra xem tệp có tồn tại không
                if (!System.IO.File.Exists(htmlFilePath))
                    return NotFound("HTML template file not found.");

                // Đọc nội dung tệp HTML
                var htmlContent = await System.IO.File.ReadAllTextAsync(htmlFilePath);

                    var convertHtml = await XuLyHoaDonThanhToan(htmlContent,request.idOrder,account.Result.Email);
                // Gửi email
                await _emailService.SendEmail(account.Result.Email, "Hoá đơn điện tử", convertHtml);

                return Ok("Email sent successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending the email: {ex.Message}");
            }
        }

        [HttpPost("send-email-with-qr")]
        public async Task<IActionResult> SendEmailWithQrCode([FromBody] EmailWithQrRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Invalid email request.");

            try
            {
                await _emailService.SendEmailAsync(request.Email, request.Subject, request.HtmlContent, request.QrCodeImage, request.QrCodeFileName);
                return Ok("Email with QR code sent successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending the email: {ex.Message}");
            }
        }
        async Task<string> XuLyHoaDonThanhToan(string htmlContent,int idThanhToan,string email)
        {
            //lấy thanh toán

            var bill = await _uow.OrderReponsitory.GetOrderById(idThanhToan);
            if (bill == null)
            {
                return "";
            }
            htmlContent = htmlContent.Replace("{{MaHoaDon}}", bill.OrderCode.ToString());
            htmlContent = htmlContent.Replace("{{NgayTaoHoaDon}}", DateTime.Now.ToString());

            //gán thông tin nhân viên và khách hàng
            htmlContent = htmlContent.Replace("{{TenKhachHangThanhToan}}", bill.FullName);
            htmlContent = htmlContent.Replace("{{SoDienThoaiKhachHangThanhToan}}", bill.NumberPhone);
            htmlContent = htmlContent.Replace("{{EmailKhachHangThanhToan}}",email);


            //lấy dịch vụ

            if (bill.DetailOrder.Count > 0)
            {

                int startTrIndex = htmlContent.IndexOf("<tr class=\"item tr_item_DichVu\">");
                int endTrIndex = htmlContent.IndexOf("</tr>", startTrIndex) + 5; // +5 để bao gồm cả thẻ </tr>
                string trContent = htmlContent.Substring(startTrIndex, endTrIndex - startTrIndex);
                // Biến để lưu nội dung HTML cuối cùng
                StringBuilder finalHtmlContent = new StringBuilder();

                // Lặp qua từng mục trong danh sách
                foreach (var item in bill.DetailOrder)
                {
                    // Sao chép nội dung HTML gốc
                    string itemHtmlContent = string.Copy(trContent);
                    // Thay thế các placeholder trong HTML với dữ liệu thực tế
                    itemHtmlContent = itemHtmlContent.Replace("{{TenDichVu}}", item.Product.ProductName + item.DetailProduct.Size);
                    itemHtmlContent = itemHtmlContent.Replace("{{size}}", item.DetailProduct.Size);
                    itemHtmlContent = itemHtmlContent.Replace("{{DonGia}}", item.DetailProduct.Price.ToString());
                    itemHtmlContent = itemHtmlContent.Replace("{{soLuong}}", item.Quantity.ToString());
                    itemHtmlContent = itemHtmlContent.Replace("{{TongTien}}", (Convert.ToInt32(item.Quantity) * Convert.ToInt32(item.DetailProduct.Price)).ToString());
                    finalHtmlContent.Append(itemHtmlContent);
                }

                htmlContent = htmlContent.Replace("{{TongTienThanhToan}}", bill.TotalPrice.ToString());
              

                htmlContent = htmlContent.Replace(trContent, finalHtmlContent.ToString());
            }





            return htmlContent;
        }
    }

    public class EmailRequest
    {
        public int accountId { get; set; }
        public int idOrder { get; set; }
     
    }

    public class EmailWithQrRequest
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public byte[] QrCodeImage { get; set; }
        public string QrCodeFileName { get; set; }
    }

}
