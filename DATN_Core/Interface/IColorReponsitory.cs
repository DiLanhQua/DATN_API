using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface IColorReponsitory : IGenericeReponsitory<Color>
    {

        public Task<ReturnColorDTO> GetAllAsync(Params colorParams);
        public Task<Color> GetColor(int idcolor);
        public Task<bool> AddColor(ColorDTO colordto);
        public Task<bool> UpdateColor(int id, ColorDTO colordto);
        public Task<bool> DeleteColor(int id);
    }
}
