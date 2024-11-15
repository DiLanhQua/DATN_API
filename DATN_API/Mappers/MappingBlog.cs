using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;

namespace DATN_API.Mappers
{
    public class MappingBlog : Profile
    {
        public MappingBlog()
        {
            CreateMap<Blog, BlogDTO>()
             .ForMember(b => b.HeadLine, o => o.MapFrom(s => s.HeadLine))
             .ReverseMap();
            CreateMap<CreateBlogDTO, Blog>().ReverseMap();
            CreateMap<UpdateDTO, Blog>().ReverseMap();

        }
    }
}
