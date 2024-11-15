using DATN_Core.DTO;
using DATN_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface IDetailOrderReponsitory : IGenericeReponsitory<DetailOrder>
    {
        Task<ReturnDetailOrder> GetAllAsync();
        Task<bool> AddAsync(CreateDetailOrder orderDTO);
    }
}

