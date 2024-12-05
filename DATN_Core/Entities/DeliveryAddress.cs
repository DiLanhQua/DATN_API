using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class DeliveryAddress: BasicEntity<int>
    {
        //public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ZipCode {  get; set; }
        public string Phone { get; set; }
        public string Note { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
