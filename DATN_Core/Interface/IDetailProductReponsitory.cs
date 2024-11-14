using DATN_Core.Entities;
using DATN_Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;

namespace DATN_Core.Interface
{
    public interface IDetailProductReponsitory : IGenericeReponsitory<DetailProduct>
    {

        Task<ReturnProductDetailDTO> GetAllAsync(BrandParams brandParams);

        Task<bool> AddAsync(ProductDetailDTO ProDTO);
        Task<bool> UpdateAsync(int id, ProductDetailDTO ProDTO);
        Task<bool> Deleteproduct(int id);


    }
}
