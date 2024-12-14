using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Product: BasicEntity<int>
    {
        //public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty ;
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int BrandId { get; set; }
        public bool Status { get; set; } = true;
        public virtual Brand Brand { get; set; }
        public virtual ICollection<DetailProduct> DetailProduct { get; set; } = new HashSet<DetailProduct>();
        public virtual ICollection<Media> Media { get; set; } = new HashSet<Media>();
        public virtual ICollection<Comment> Comment { get; set; } = new HashSet<Comment>();
        public virtual ICollection<WishList> WishList { get; set; } = new HashSet<WishList>();
    }
}
