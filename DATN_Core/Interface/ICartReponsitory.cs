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
    public interface ICartReponsitory : IGenericeReponsitory<Cart>
    {
        Task<ReturnCartDTO> GetAllAsync(BrandParams brandParams);
        Task<bool> AddAsync(CreartCart cartDTO);
        Task<bool> UpdateCart(int id, UpCart cartDTO);
        Task<bool> DeleteCart(int id);
    }
}
