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
    public interface IProductReponsitory : IGenericeReponsitory<Product>
    {
        Task<ReturnProductDTO> GetAllAsync(Params brandParams);
        Task<bool> AddAsync(ProductDTO ProDTO);
        Task<bool> Updateproduct(int id, ProductDTO ProDTO);
        Task<bool> Deleteproduct(int id);
       
    }
}
