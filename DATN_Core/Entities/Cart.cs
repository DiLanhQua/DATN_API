using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Cart: BasicEntity
    {
        //public int Id { get; set; }
        public DateTime TimeCreated { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<DetailCart> DetailCart { get; set; } = new HashSet<DetailCart>();


    }
}
