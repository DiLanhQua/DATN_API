using DATN_Core.Entities;
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

    public class AddCommentDTO
    {
        public int Rating { get; set; }
        public string Content { get; set; }
        public int ProductId { get; set; }
        public int DetailProductId { get; set; }
        public int AccountId { get; set; }
    }

    public class CheckIsCommentDTO
    {
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public int DetailProductId { get; set; }
    }

    public class GetCommentDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public virtual GetCommentDTO_Account Account { get; set; }
        public virtual GetCommentDTO_DetailProduc DetailProduct { get; set; }
    }

    public class GetCommentDTO_Account
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

    }

    public class GetCommentDTO_DetailProduc
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Gender { get; set; }
    }

}
