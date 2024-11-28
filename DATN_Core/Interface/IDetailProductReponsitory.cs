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

        Task<ReturnProductDetailDTO> GetAllAsync(Params brandParams);

        Task<List<ProductDetailDE>> GetProductDetail(int productid);
        Task<bool> AddAsync(ProductDetailDTO ProDTO);
        Task<bool> UpdateAsync(int id, int idproduct, ProductDetailUP ProDTO);
        Task<bool> Deleteproduct(int id);
        Task<ProductDetailDE> GetDetail(int id, int productid);

    }
}
