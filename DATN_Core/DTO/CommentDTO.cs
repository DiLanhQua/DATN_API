using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public int ProductId { get; set; }
        public int AccountId { get; set; }
    }

    public class UpdateCommentDTO
    {
        public string Content { get; set; }
    }

    public class CommentDTO
    {
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public string ProductName { get; set; } // Nếu cần lấy tên sản phẩm
        public string AccountName { get; set; } // Nếu cần lấy tên tài khoản
    }
    public class ReturnCommentDTO
    {
        public int totalItems { get; set; }
        public List<CommentDTO> CommentsDTO { get; set; }
    }
}
