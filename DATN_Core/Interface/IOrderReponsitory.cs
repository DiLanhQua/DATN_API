﻿using DATN_Core.DTO;
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
        Task<int> AddAsync(CreateOrder orderDTO);
        Task<bool> UpdateOrder(int id, UpdateOrder orderDTO);

        Task<List<Order>> GetAllOrder();

        Task<List<OrderUserDtos>> GetOrderByIdUser(int idUser);
        Task<OrderUserForDetailDtos> GetOrderById(int id);
    }
}
