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
    public interface IBrandReponsitory : IGenericeReponsitory<Brand>
    {
        Task<ReturnBrandDTO> GetAllAsync(BrandParams brandParams);
        Task<bool> AddAsync (CreateBrandDTO brandDTO);
        Task<bool> UpdateAsync(int id, UpdateBrandDTO brandDTO);
    }
}
