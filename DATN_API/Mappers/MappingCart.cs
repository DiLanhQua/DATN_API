using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingCart : Profile
    {

        public MappingCart()
        {
            //CreateMap<CartDe, CartDTO>().ReverseMap();
            CreateMap<CartDe, DetailCart>().ReverseMap();
        }

    }
}
