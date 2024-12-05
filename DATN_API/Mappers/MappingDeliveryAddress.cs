using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;

namespace DATN_API.Mappers
{
    public class MappingDeliveryAddress : Profile
    {
        public MappingDeliveryAddress() 
        {
            CreateMap<CreateDeliveryDtos, DeliveryAddress>().ReverseMap();
        }
    }
}
