using DATN_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string OrderCode { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public DateTime TimeOrder { get; set; }

        public int StatusOrder { get; set; }

        public string PaymentMethod { get; set; }

        public int? VoucherId { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string OrderStatus { get; set; } = string.Empty;
    }

    public class CreateOrder : OrderDTO
    {

    }

    public class UpdateOrder
    {
        public int StatusOrder { get; set; }
        public string? Reason { get; set; }
    }


    public class ReturnOrder
    {
        public List<OrderDTO> Orders { get; set; }
    }
}