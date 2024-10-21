using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public virtual ICollection<Product> Product { get; set;} = new HashSet<Product>();
    }
}
