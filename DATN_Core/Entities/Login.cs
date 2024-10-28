using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public  class Login: BasicEntity
    {
        //public int Id { get; set; }

        public string Action { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }

        public string Description { get; set; } = "";

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

    }
}
