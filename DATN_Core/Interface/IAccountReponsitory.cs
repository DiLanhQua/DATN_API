using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface IAccountReponsitory : IGenericeReponsitory<Account>
    {
        Task<ReturnAccountDTO> GetAllAsync(BrandParams brandParams);
        Task<ReturnAccountDTO> GetAllUsers(string trangthai, BrandParams brandParams);
        Task<bool> AddAccount(CreartAccount accountDTO);
        Task<bool> UpAccount(int idaccount, UpAccount upAccount);
        Task<LoginCT> GetDeLogin(int idaccount);
        Task<AccountDTO> GetDeAccount(int idaccount);
        Task<int> XNAccount(int idaccount);
        public Task<int> LockAccount(int idaccount, string lydo);
        public Task<int> OpenAccount(int idaccount);
        public Task XoaTKHH();
    }
}
