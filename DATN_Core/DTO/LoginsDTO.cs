using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class LoginsDTO
    {

        public string Action { get; set; } = string.Empty;


        public string Description { get; set; } = "";

        public int AccountId { get; set; }
    }

    public class ListLoginsDTO
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }

        public string Description { get; set; } = "";

        public int AccountId { get; set; }
    }

    public class ReturnLogin
    {
        public int totalItems { get; set; }
        public List<ListLoginsDTO> ListLogins { get; set; } = new List<ListLoginsDTO>();
    }

    public class LoginAccountUser
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }


    public class RegisterDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte Role { get; set; }
        public int Status { get; set; } = 0;
    }

    public class ProfileDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        public int Status { get; set; } = 0;
    }

    public class UpProfile
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
}
