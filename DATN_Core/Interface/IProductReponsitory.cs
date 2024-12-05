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
        Task<ProductsUserReturnDtos> GetAllUserAsync(Params brandParams);
        Task<ProductDEDTO> GetProduct(int id);
        Task<bool> AddProduct(ProductDTO ProDTO);
        Task<bool> UpdateProduct(int id, ProductUPDTO productUP);
        Task<bool> Deleteproduct(int id);
       
    }
}
