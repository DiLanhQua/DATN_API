using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;
using AutoMapper;
using DATN_API.Helper;

namespace DATN_API.Mappers
{
    public class MappingLogin : Profile
    {
        public MappingLogin() { 
            
            CreateMap<Login, LoginDTO>().ReverseMap();
            CreateMap<LoginDTO, Login>().ReverseMap();
        
        }
    }
}
