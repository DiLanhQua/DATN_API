using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Sharing
{
    public class QrCoder
    {
        public async Task<byte[]> QRCodeAsync(string content)
        {
            using(var qrCoder = new QRCodeGenerator())
            {
                var qr = qrCoder.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                using(var qrCode = new PngByteQRCode(qr))
                {
                    return await Task.FromResult(qrCode.GetGraphic(20));
                }
            }
        }
    }
}
