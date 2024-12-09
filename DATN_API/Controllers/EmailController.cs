using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DATN_Core.Interface;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmail _emailService;

        public EmailController(IEmail emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Invalid email request.");

            try
            {
                await _emailService.SendEmail(request.Email, request.Subject, request.HtmlContent);
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
    }

    public class EmailRequest
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
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
