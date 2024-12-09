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
        Task<ReturnBlogDTO> GetAllAsync(Params brandParams);

        Task<BlogDTO> GetBlogByIdAsync(int id);

        Task<List<ImageBlogDtos>> GetImageByIdBlog(int idBlog);
        
        Task<bool> AddAsync(CreateBlogDTO blogDTO);
        
        Task<bool> UpdateAsync(int id, CreateBlogDTO BlogDTO);

        Task<bool> DeleteImageBlog(int idImage);

        Task<bool> IsPrimaryBlog(int idImage);

        Task<bool> DeleteBlogById(int id);
    }
}
