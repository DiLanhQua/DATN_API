using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingAccount: Profile
    {
        public  MappingAccount()
        {
            
            CreateMap<Account, AccountDTO>()
            .ForMember(b => b.Image, o => o.MapFrom<AccountResolver>())
            .ReverseMap();

            CreateMap<LoginCT, Login>().ReverseMap();
        }
    }
}
