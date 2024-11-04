using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingOrder : Profile
    {
        public MappingOrder()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDTO, Order>().ReverseMap();
        }
    }

}
