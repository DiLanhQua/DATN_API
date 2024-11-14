using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.DTO
{
    public class ProductDTO
    {
       // public int? Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
       
    }


    public class ReturnProductDTO
    {
        public int TotalItems { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
