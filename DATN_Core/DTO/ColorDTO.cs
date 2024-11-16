using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class ColorDTO
    {
        public string NameColor { get; set; }

        public string ColorCode { get; set; }
    }

    public class ReturnColorDTO
    {
        public int TotalItems { get; set; }
        public List<ColorDTO> ColorList { get; set; }
    }
}
