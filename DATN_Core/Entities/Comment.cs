using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Comment:BasicEntity
    {
        //public int Id { get; set; }
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }



    }
}
