using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class DetailOrder: BasicEntity
    {
        //public int Id { get; set; }
        public byte Quantity { get; set; }
        public int DetailProductId { get; set; }
        public virtual DetailProduct DetailProduct { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
