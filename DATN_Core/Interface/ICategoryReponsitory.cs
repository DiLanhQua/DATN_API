using DATN_Core.Entities;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface ICategoryReponsitory : IGenericeReponsitory<Category>
    {
        Task<ReturnCatagoryDTO> GetAllAsync(Params brandParams);
        Task<bool> AddAsync(CreateCatagoryDTO catagoryDTO);
        Task<bool> UpdateAsync(int id, UpdateCatagoryDTO catagoryDTO);
    }
}
