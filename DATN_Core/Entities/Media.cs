using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Media: BasicEntity
    {
        //public int Id { get; set; }
        public bool IsPrimary { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }

    }
}
