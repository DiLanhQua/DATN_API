using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class DetailProduct: BasicEntity<int>
    {
        public int Id { get; set; }

        public string Size { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public string Gender { get; set; }

        public string Status {  get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int ColorId { get; set; }

        public virtual Color Color { get; set; }
        public virtual ICollection<DetailCart> DetailCart { get; set; } = new HashSet<DetailCart>();
        public virtual ICollection<DetailOrder> DetailOrder { get; set; } = new HashSet<DetailOrder>();





    }
}
