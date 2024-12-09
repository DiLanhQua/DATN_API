using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Blog: BasicEntity <int>
    {
        //public int Id { get; set; }
        public string HeadLine { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; }
        
        public DateTime DatePush { get; set; }
        
        public int AccountId { get; set; }
        
        public virtual Account Account { get; set; }
        
        public virtual ICollection<Media> Media { get; set; } = new HashSet<Media>();
    }
}
