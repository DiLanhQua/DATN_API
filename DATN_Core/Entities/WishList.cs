using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class WishList
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public DateTime DateAdd { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

    }
}
