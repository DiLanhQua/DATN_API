using AutoMapper;
using DATN_API.Helper;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingCategory: Profile
    {
        public MappingCategory()
        {
            CreateMap<Category, CataDTO>()
               .ForMember(b => b.CategoryName, o => o.MapFrom(s => s.CategoryName))
               .ForMember(b => b.Image, o => o.MapFrom<CategoryResolvel>())
               .ReverseMap();
            CreateMap<CreateCatagoryDTO, Category>().ReverseMap();
            CreateMap<UpdateCatagoryDTO, Category>().ReverseMap();

        }
    }
}
