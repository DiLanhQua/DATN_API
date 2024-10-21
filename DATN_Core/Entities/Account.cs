using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public string Password { get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte Role { get; set; } 
        public string Image {  get; set; } = string.Empty;
        public virtual ICollection<Login> Login { get; set; } = new HashSet<Login>();
        public virtual ICollection<Order> Order { get; set; } = new HashSet<Order>();
        public virtual ICollection<Cart> Cart { get; set; } = new HashSet<Cart>();
        public virtual ICollection<Comment> Comment { get; set; } = new HashSet<Comment>();
        public virtual ICollection<WishList> WishList { get; set; } = new HashSet<WishList>();
        public virtual ICollection<Blog> Blog { get; set; } = new HashSet<Blog>();



    }
}
