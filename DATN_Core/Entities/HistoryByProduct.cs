using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class HistoryByProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int DetailProductId { get; set; }
        [ForeignKey("DetailProductId")]
        public DetailProduct DetailProduct { get; set; }
        public int Status { get; set; } = 1;
    }
}
