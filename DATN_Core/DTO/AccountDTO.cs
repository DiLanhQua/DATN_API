using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class AccountDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte Role { get; set; }
        public string Image { get; set; }
    }

    public class AccountCT
    {
        
        public string Action { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }

        public string Description { get; set; } = "";

        public int AccountId { get; set; }
    }
    public class LoginDTO
    {
        public int AccountId { get; set; }
    }
    public class CreartAccount
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte Role { get; set; }
        public IFormFile Picture { get; set; }
    }
    public class UpAccount
    {
        public int AccountId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte Role { get; set; }
        public IFormFile Picture { get; set; }
    }
    public class ReturnAccountDTO
    {
        public int totalItems { get; set; }
        public List<AccountDTO> AccountsDTO { get; set; }
    }
}
