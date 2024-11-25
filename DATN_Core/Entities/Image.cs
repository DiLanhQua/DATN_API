using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Image: BasicEntity<int>
    {
        //public int Id { get; set; }
        public string Link { get; set; } = string.Empty;
        public virtual ICollection<Media> Media { get; set; } = new HashSet<Media>();

    }
}
