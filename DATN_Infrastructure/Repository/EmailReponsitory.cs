using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using DATN_Infrastructure.Data.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;
namespace DATN_Infrastructure.Repository
{
    public class EmailReponsitory : IEmail
    {
        private readonly EmailDTO _emailDTO;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public EmailReponsitory(IOptions<EmailDTO> emailDTO,ApplicationDbContext applicationDbContext,IMapper mapper) 
        {
            _emailDTO = emailDTO.Value;
            _context = applicationDbContext;
            _mapper = mapper;
        }
        public async Task SendEmail(string email, string subject, string htmlContent)
        {
            var mess = new MimeMessage();
            mess.From.Add(new MailboxAddress("Lạnh quá đi", _emailDTO.Username ?? throw new ArgumentNullException(nameof(_emailDTO.Username))));

            mess.To.Add(new MailboxAddress("", email));
            mess.Subject = subject;

            var bodyEmail = new BodyBuilder();
            bodyEmail.HtmlBody = htmlContent;
            mess.Body = bodyEmail.ToMessageBody();
            using(var ms = new SmtpClient())
            {
                await ms.ConnectAsync(_emailDTO.SmtpServer, _emailDTO.SmtpPort, SecureSocketOptions.StartTls);
                await ms.AuthenticateAsync(_emailDTO.Username, _emailDTO.Password);
                await ms.SendAsync(mess);
                await ms.DisconnectAsync(true);
            }
        }
        public async Task SendEmailAsync(string to, string subject, string htmlContent, byte[] qrCodeImage, string qrCodeFileName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("COZAStore", _emailDTO.Username));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = htmlContent;

            bodyBuilder.LinkedResources.Add(qrCodeFileName, qrCodeImage);

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                await client.ConnectAsync(_emailDTO.SmtpServer, _emailDTO.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailDTO.Username, _emailDTO.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<bool> SendEmaiHoaDonlAsync(EmailRequest request)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Id==request.AccoutID);

            if (request == null || account == null)
                return false;

            try
            {
                var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-templates", "template.html");
                if (!System.IO.File.Exists(htmlFilePath))
                    return false;

                var htmlContent = await System.IO.File.ReadAllTextAsync(htmlFilePath);
                var convertedHtml = await XuLyHoaDonThanhToan(htmlContent, request.idOrder, account.Email);

                await SendEmail(account.Email, "Hoá đơn điện tử", convertedHtml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> XuLyHoaDonThanhToan(string htmlContent, int idThanhToan, string email)
        {
            //lấy thanh toán1

            var bill = await GetOrderById(idThanhToan);
            if (bill == null)
            {
                return "";
            }
            htmlContent = htmlContent.Replace("{{MaHoaDon}}", bill.OrderCode.ToString());
            htmlContent = htmlContent.Replace("{{NgayTaoHoaDon}}", DateTime.Now.ToString());

            //gán thông tin nhân viên và khách hàng
            htmlContent = htmlContent.Replace("{{TenKhachHangThanhToan}}", bill.FullName);
            htmlContent = htmlContent.Replace("{{SoDienThoaiKhachHangThanhToan}}", bill.NumberPhone);
            htmlContent = htmlContent.Replace("{{EmailKhachHangThanhToan}}", email);


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
                    itemHtmlContent = itemHtmlContent.Replace(
    "{{TongTien}}",
    (Convert.ToInt32(item.Quantity) * Convert.ToInt32(item.DetailProduct.Price)).ToString()
);




                    finalHtmlContent.Append(itemHtmlContent);
                }

                htmlContent = htmlContent.Replace("{{TongTienThanhToan}}", bill.TotalPrice.ToString());


                htmlContent = htmlContent.Replace(trContent, finalHtmlContent.ToString());
            }





            return htmlContent;
       
        }
        public async Task<OrderUserForDetailDtos> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Where(x => x.Id == id)
                .Include(a => a.Voucher)
                .Include(a => a.DeliveryAddress)
                .Include(a => a.Account)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return null;
            }

            var detailOrder = await _context.DetailOrders
                .Where(x => x.OrderId == order.Id)
                .Include(a => a.DetailProduct)
                .ToListAsync();

            var deliveryAddress = order.DeliveryAddress?.FirstOrDefault();

            List<DetailOrderDtoForOrder> detailOrderRes = new List<DetailOrderDtoForOrder>();

            foreach (var item in detailOrder)
            {
                var media = await _context.Medium
                .Where(a => a.ProductId == item.DetailProduct.ProductId && a.IsPrimary == true)
                .Join(
                    _context.Images,
                    medium => medium.ImagesId,
                    image => image.Id,
                    (medium, image) => new ImageDeDTO
                    {
                        Id = medium.Id,
                        IsImage = medium.IsPrimary,
                        Link = image.Link
                    }
                )
                .FirstOrDefaultAsync();

                List<MediaADD> mediaADDs = new List<MediaADD>();

                mediaADDs.Add(new MediaADD
                {
                    Link = media.Link,
                    IsPrimary = media.IsImage,
                });

                var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == item.DetailProduct.ProductId);

                ProductDTO productDTO = new ProductDTO
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    BrandId = product.BrandId,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    Medias = mediaADDs,
                };

                DetailOrderDtoForOrder detailOrderDtos = new DetailOrderDtoForOrder
                {
                    DetailProductId = item.DetailProductId,
                    Quantity = item.Quantity,
                    OrderId = item.OrderId,
                    DetailProduct = _mapper.Map<ProductDetailDTO>(item.DetailProduct),
                    Product = productDTO
                };

                detailOrderRes.Add(detailOrderDtos);
            }

            // Ánh xạ các DetailOrder thành DTO

            var result = new OrderUserForDetailDtos
            {
                Id = order.Id,
                OrderCode = $"DH{order.Id.ToString().PadLeft(4, '0')}",
                FullName = order.Account?.FullName,
                NumberPhone = deliveryAddress?.Phone,
                Status = order.StatusOrder,
                Address = deliveryAddress?.Address,
                Voucher = _mapper.Map<VoucherDTO>(order.Voucher),
                DetailOrder = detailOrderRes,
                TotalPrice = order.Total,
            };

            return result;
        }
    }
}
