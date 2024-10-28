using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class DetailCart: BasicEntity
    {
        //public int Id { get; set; }
        public byte Quantity { get; set; }
        public int DetailProductId { get; set; }
        public virtual DetailProduct DetailProduct { get; set; }
        public int CartId { get; set; }
        public virtual Cart Carts { get; set; }
    }
}
