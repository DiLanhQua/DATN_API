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
        public List<ProductDetailDTO> ProductDetails { get; set; } = new List<ProductDetailDTO>();
    }

    public class ProductDetailDTO
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        //public int ProductId { get; set; }
    }

    public class CreateProductDTO : ProductDTO
    {
        public List<ProductDetailDTO> ProductDetails { get; set; } = new List<ProductDetailDTO>();
    }

    public class UpdateProductDTO : ProductDTO
    {
        public List<ProductDetailDTO> ProductDetails { get; set; } = new List<ProductDetailDTO>();
    }

    public class ReturnProductDTO
    {
        public int TotalItems { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
