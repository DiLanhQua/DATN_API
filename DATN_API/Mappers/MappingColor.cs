using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingColor : Profile
    {
        public MappingColor() { 
            CreateMap<Color, ColorDTO>().ReverseMap();
            CreateMap<ColorDTO, Color>().ReverseMap();
        
        }
    }
}
