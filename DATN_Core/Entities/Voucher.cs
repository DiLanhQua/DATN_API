using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public  class Voucher: BasicEntity<int>
    {
        //public int Id { get; set; }

        public string VoucherName { get; set; }
        public DateTime TimeStart { get; set; }

        public DateTime TimeEnd { get; set; }

        public string DiscountType { get; set; } = string.Empty;
            
        public byte Quantity { get; set; }

        public int Discount { get; set; }

        public int Min_Order_Value { get; set; }

        public int Max_Discount { get; set; }

        public byte Status { get; set; }
        public virtual ICollection<Order> Order { get; set; } = new HashSet<Order>();





    }
}
