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
    public interface IBlogReponsitory : IGenericeReponsitory<Blog>
    {
        Task<ReturnBlogDTO> GetAllAsync(BrandParams brandParams);
        Task<bool> AddAsync(CreateBlogDTO blogDTO);
        Task<bool> UpdateAsync(int id, UpdateDTO BlogDTO);
    }
}
