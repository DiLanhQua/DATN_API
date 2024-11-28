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
    public interface IMediaReponsitory : IGenericeReponsitory<Media>
    {
        Task<Media> GetMedia(int idproduct);
        Task<List<Media>> GetALLMedia(int idproduct);
        Task<bool> AddMedia(int productId, MediaADD mediaAdd);
        Task<bool> DeleteMedia(int mediaId);
        Task<bool> ChangeImage(int productId, int imageId, MediaADD mediaAdd);
        Task<bool> SetPrimaryImage(int productId, int imageId);
    }
}
