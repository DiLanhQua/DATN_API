using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        public string BrandName { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string Image {  get; set; } = string.Empty;
        public virtual ICollection<Product> Product { get; set; } = new HashSet<Product>();

    }
}
