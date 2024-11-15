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
    }
}
