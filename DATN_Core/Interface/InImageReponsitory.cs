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
<<<<<<< Updated upstream
        Task<bool> AddAsync(CreateImageDTO brandDTO);
=======
        Task<bool> AddAsync(CreateImageDTO brandDTO); 
>>>>>>> Stashed changes
        Task<ImageDeDTO> GetImage(int idproduct);
    }
}
