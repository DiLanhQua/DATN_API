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
    public interface IOrderReponsitory : IGenericeReponsitory<Order>
    {
        Task<ReturnOrder> GetAllAsync();
        Task<bool> AddAsync(CreateOrder orderDTO);
        Task<bool> UpdateOrder(int id, UpdateOrder orderDTO);
    }
}
