using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;

namespace DATN_API.Mappers
{
    public class MappingProductDetail : Profile
    {
        public MappingProductDetail()
        {
            
            CreateMap<DetailProduct, ProductDetailDTO>().ReverseMap();
            CreateMap<ProductDetailDTO,DetailProduct >().ReverseMap();
<<<<<<< Updated upstream
            CreateMap<DetailProduct, DetailProduct>();

=======
            CreateMap<DetailProduct, ProductDetailDE>().ReverseMap();
>>>>>>> Stashed changes


        }
    }
}
