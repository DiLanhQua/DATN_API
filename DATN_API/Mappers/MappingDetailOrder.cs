using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;

namespace DATN_API.Mappers
{
    public class MappingDetailOrder : Profile
    {
        public MappingDetailOrder()
        {
            CreateMap<DetailOrder, DetailOrderDTO>().ReverseMap();
            CreateMap<DetailOrderDTO, DetailOrder>().ReverseMap();
        }
    }

}
