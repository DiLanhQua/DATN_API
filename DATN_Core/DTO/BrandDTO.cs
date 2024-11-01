using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.DTO
{
    public class BaseBrand
    {
        [Required]
        public string BrandName { get; set; }
        public string Country { get; set; }

    }
    public class BrandDTO: BaseBrand
    {
        public int Id { get; set; }
        public string Image { get; set; }

    }

    public class CreateBrandDTO: BaseBrand
    {
        public IFormFile Picture { get; set; }

    }
    public class ReturnBrandDTO
    {
        public int totalItems { get; set; }
        public List<BrandDTO> BrandsDTO { get; set; }
    }
    public class UpdateBrandDTO : BaseBrand
    {
        public string oldImage { get; set; }
        public IFormFile Picture { get; set;}
    }


}
