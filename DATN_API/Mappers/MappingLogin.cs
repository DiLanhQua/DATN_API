using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;

namespace DATN_API.Mappers
{
    public class MappingLogin : Profile
    {
        public MappingLogin()
        {
            CreateMap<Login, LoginsDTO>().ReverseMap();
            CreateMap<Login, ListLoginsDTO>().ReverseMap();
        }
    }
}
