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

        public string FullName { get; set; }

        public string Image {  get; set; }

    }

    public class CreateBlogDTO
    {
        public string HeadLine { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int AccountId { get; set; }

        public List<ImageBlog>? Images { get; set; }

    }

    public class ImageBlog
    {
        public bool IsPrimary { get; set; }

        public string Url { get; set; } = string.Empty;
    }

    public class ImageBlogDtos
    {
        public int Id { get; set; }
    
        public string Link { get; set; }

        public bool IsPrimary { get; set; }
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
