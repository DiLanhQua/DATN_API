using DATN_Infrastructure.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class ProductDetailDTO
    {
        public string Size { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int ColorId { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public int ProductId { get; set; }

    }
    public class ProductDetailDE
    {
        public int Id { get; set; }
        public string Size { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int ColorId { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public int ProductId { get; set; }

    }

    public class ReturnProductDetailDTO
    {
        public int TotalItems { get; set; }
        public List<ProductDetailDTO> Productdetail { get; set; } = new List<ProductDetailDTO>();
    }




}
