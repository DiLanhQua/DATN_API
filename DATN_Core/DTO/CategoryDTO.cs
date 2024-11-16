using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.DTO
{
    public class BaseCatagory
    {
        [Required]
        public string CategoryName { get; set; }

    }
    public class CataDTO : BaseCatagory
    {
        public int Id { get; set; }
        public string Image { get; set; }

    }

    public class CreateCatagoryDTO : BaseCatagory
    {
        public IFormFile Picture { get; set; }

    }
    public class ReturnCatagoryDTO
    {
        public int totalItems { get; set; }
        public List<CataDTO> cataDTOs { get; set; }
    }
    public class UpdateCatagoryDTO : BaseCatagory
    {
        public string oldImage { get; set; }
        public IFormFile Picture { get; set; }
    }

}
