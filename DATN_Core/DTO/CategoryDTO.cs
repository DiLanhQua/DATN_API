using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.DTO
{
    public class CategoryDTO
    {
        [Required]
        public string CategoryName { get; set; }
        public string Image { get; set; }
    }
    public class ListCategoryDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
    }
}
