using DATN_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class DetailOrderDTO
    {
        public int DetailProductId { get; set; } // Corresponds to the DetailProductId in DetailOrder
        public byte Quantity { get; set; } // Corresponds to the Quantity in DetailOrder
        public int OrderId { get; set; }
    }
    public class CreateDetailOrder : DetailOrderDTO
    {
    }
    public class UpdateDetailOrder
    {
        public int StatusOrder { get; set; }
    }
    public class ReturnDetailOrder
    {
        public List<DetailOrderDTO> DetailOrders { get; set; }
    }
}

