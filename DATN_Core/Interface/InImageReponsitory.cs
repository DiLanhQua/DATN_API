using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface InImageReponsitory: IGenericeReponsitory<Image>
    {
        Task<bool> AddAsync(CreateImageDTO brandDTO); 
        Task<ImageDeDTO> GetImage(int idproduct);
    }
}
