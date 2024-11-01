using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class CartDTO
    {
        public byte Quantity { get; set; }
        public int DetailProductId { get; set; }
    }

    public class CartDe
    {
        public string Color { get; set; }

        public string Gender { get; set; }

        public string Size { get; set; }

        public int Price { get; set; }
        public byte Quantity { get; set; }
        public int DetailProductId { get; set; }
    }
    public class CreartCart
    {
        public int AccountId { get; set; }
        public List<CartDTO> CartsDTO { get; set; }
    }
    public class UpCart
    {
        public byte Quantity { get; set; }
    }
    public class ReturnCartDTO
    {
        public int totalItems { get; set; }
        public List<CartDe> CartsDTO { get; set; }
    }
}
