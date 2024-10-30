using AutoMapper;
using DATN_API.Helper;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;
namespace DATN_API.Mappers
{
    public class MappingBrand: Profile
    {
        public MappingBrand()
        {
            //CreateMap<BrandDTO,Brand>().ReverseMap();
            CreateMap<Brand, BrandDTO>()
             .ForMember(b => b.BrandName, o => o.MapFrom(s => s.BrandName))
             .ForMember(b => b.Image, o => o.MapFrom<BrandResolver>())
             .ReverseMap();
            CreateMap<CreateBrandDTO, Brand>().ReverseMap();
            CreateMap<UpdateBrandDTO, Brand>().ReverseMap();

        }
    }
}
