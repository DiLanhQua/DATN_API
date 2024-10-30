using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Sharing
{
    public class BrandParams
    {
        public int maxPageSize { get; set; } = 5;
        private int pageSize = 3;
        public int Pagesize
        {
            get => pageSize;
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }
        public int PageNumber { get; set; } = 1;
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
