using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface IEmail
    {
        Task SendEmail(string email, string subject, string htmlContent);
        Task SendEmailAsync(string to, string subject, string htmlContent, byte[] qrCodeImage, string qrCodeFileName);
        Task<string> XuLyHoaDonThanhToan(string htmlContent, int idThanhToan, string email);
        Task<bool> SendEmaiHoaDonlAsync(EmailRequest request);
    }
    public class EmailRequest
    {
        public int AccoutID { get; set; }
        public int idOrder { get; set; }

    }
}
