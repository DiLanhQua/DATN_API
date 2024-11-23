using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Infrastructure.Data.DTO;
using AutoMapper;
using DATN_API.Helper;


namespace DATN_API.Mappers
{
    public class MappingImages : Profile
    {
        public MappingImages()
        {
<<<<<<< Updated upstream
            CreateMap<Image, ImageDeDTO>()           
=======
            //CreateMap<Image, ImageDTO>()           
            // .ForMember(b => b.Link, o => o.MapFrom<ImageResolver>())
            // .ReverseMap();
            CreateMap<Image, ImageDeDTO>()
>>>>>>> Stashed changes
             .ForMember(b => b.Link, o => o.MapFrom<ImageResolver>())
             .ReverseMap();
            CreateMap<CreateImageDTO, Image>().ReverseMap();
            CreateMap<UpdateImageDTO, Image>().ReverseMap();
        }
    }
}
