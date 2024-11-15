using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Color : BasicEntity<int>
    {
        public string NameColor { get; set; }

        public string ColorCode { get; set; }

        public virtual ICollection<DetailProduct> DetailProduct { get; set; } = new HashSet<DetailProduct>();
    }
}
