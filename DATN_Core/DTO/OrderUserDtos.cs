using DATN_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class OrderUserDtos
    {
        public int Id { get; set; }

        public string OrderCode { get; set; } = string.Empty;

        public string Paymend {  get; set; } = string.Empty ;

        public string FullName { get; set; } = string.Empty;

        public string NumberPhone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public int Status {  get; set; }

    }
    public class OrderUserForDetailDtos
    {
        public int Id { get; set; }

        public string OrderCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string NumberPhone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public int Status { get; set; }
        
        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
        
        public VoucherDTO Voucher { get; set; }

        public List<DetailOrderDtoForOrder>  DetailOrder { get; set; }
    }

}
