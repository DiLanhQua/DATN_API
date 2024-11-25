using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;

namespace DATN_API.Mappers
{
    public class MappingProductDetail : Profile
    {
        public MappingProductDetail()
        {
            
            CreateMap<DetailProduct, ProductDetailDTO>().ReverseMap();
            CreateMap<ProductDetailDTO,DetailProduct >().ReverseMap(); 
            CreateMap<DetailProduct, ProductDetailDE>().ReverseMap();


        }
    }
}
