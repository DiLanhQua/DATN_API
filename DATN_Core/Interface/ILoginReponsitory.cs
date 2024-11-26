using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface ILoginReponsitory : IGenericeReponsitory<Login>
    {
        Task<Account> Login(string username, string password);
        Task<ReturnLogin> GetAllAsync(Params LoginParams);
        Task<bool> AddAsync(LoginsDTO loginDTO);
        Task<bool> RegisterAsync(RegisterDTO registerDTO);
    }
}
