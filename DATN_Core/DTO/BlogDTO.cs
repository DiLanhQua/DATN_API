using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class BaseBlog
    {
        [Required]
        public string HeadLine { get; set; }
        public string Content { get; set; }
        public DateTime DatePush { get; set; }
    }
    public class BlogDTO : BaseBlog
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

    }

    public class CreateBlogDTO : BaseBlog
    {
        public int AccountId { get; set; }

    }
    public class ReturnBlogDTO
    {
        public int totalItems { get; set; }
        public List<BlogDTO> BlogsDTO { get; set; }
    }
    public class UpdateDTO : BaseBlog
    {
        public int AccountId { get; set; }
    }
}
