using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class BaseMedia
    {
        [Required]
        public bool IsPrimary { get; set; }
    }
    public class mediaDTO : BaseMedia
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BlogId { get; set; }
        public int ImageId { get; set; }

    }

    public class CreateMediaDTO : BaseMedia
    {
        public List<IFormFile> Picture { get; set; }
        public int ProductId { get; set; }
        public int BlogId { get; set; }
        //public int ImageId { get; set; }

    }
    public class ReturnMediaDTO
    {
        public int totalItems { get; set; }
        public List<mediaDTO> MediaDTOs { get; set; }
    }
    public class UpdateMediaDTO : BaseMedia
    {
        public string oldImage { get; set; }
        public List<IFormFile> Picture { get; set; }
        public int ProductId { get; set; }
        public int BlogId { get; set; }
    }
}
