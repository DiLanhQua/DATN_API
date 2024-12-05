using AutoMapper;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingProduct : Profile
    {
        public MappingProduct()
        {
           /* CreateMap<ProductDetailDTO, ProductDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.BrandId, opt => opt.Ignore())
                .ReverseMap();*/

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductDTO, Product > ().ReverseMap();
            CreateMap<Product, ProductDEDTO>().ReverseMap(); 
            CreateMap<ProductDEDTO, Product>().ReverseMap();
            CreateMap<Product, ProductDEDTO>().ReverseMap();

        }
    }
}
