using AutoMapper;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingCategory: Profile
    {
        public MappingCategory()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<ListCategoryDTO ,Category>().ReverseMap();
        }
    }
}
