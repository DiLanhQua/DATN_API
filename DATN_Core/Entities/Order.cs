using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Order: BasicEntity<int>
    {
        //public int Id { get; set; }
        public decimal Total { get; set; }  
        public DateTime TimeOrder { get; set; }
        public byte StatusOrder { get; set; }
        public string PaymentMethod { get; set; }
        public int VoucherId { get; set; }
        public virtual Voucher Voucher { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<DetailOrder> DetailOrder { get; set; } = new HashSet<DetailOrder>();
        public virtual ICollection<DeliveryAddress> DeliveryAddress { get; set; } = new HashSet<DeliveryAddress>();




    }
}
