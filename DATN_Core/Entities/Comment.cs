using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Comment: BasicEntity<int>
    {
        //public int Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public int DetailProductId { get; set; }
        public virtual DetailProduct DetailProduct { get; set; }
    }
}
