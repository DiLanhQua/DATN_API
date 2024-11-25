using DATN_Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.DTO
{
    public class ProductDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public List<ProductDetaiAdd> ProductDetais { get; set; } = new List<ProductDetaiAdd>();
        public List<MediaADD> Medias { get; set; } = new List<MediaADD>();
    }

    public class MediaADD
    {
        public bool IsPrimary { get; set; }

        // File ảnh hoặc video
        public string Link { get; set; }
    }

    public class ProductDetaiAdd
    {
        public string Size { get; set; } = string.Empty; // Kích thước sản phẩm
        public int Price { get; set; } // Giá sản phẩm
        public int Quantity { get; set; } // Số lượng tồn kho
        public int ColorId { get; set; } // Mã màu sản phẩm
        public string Gender { get; set; } = string.Empty; // Dành cho nam, nữ hoặc unisex
        public string Status { get; set; } = string.Empty; // Trạng thái sản phẩm
    }

    public class ProductDEDTO
    {
        public int? Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int BrandId { get; set; }

    }

    public class ReturnProductDTO
    {
        public int TotalItems { get; set; }
        public List<ProductDEDTO> Products { get; set; } = new List<ProductDEDTO>();
    }
}
