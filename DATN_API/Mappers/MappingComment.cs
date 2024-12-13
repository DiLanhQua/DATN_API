using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;

namespace DATN_API.Mappers
{
    public class MappingComment : Profile
    {
        public MappingComment()
        {
            CreateMap<CommentDTO, Comment>().ReverseMap();
            // Các cấu hình ánh xạ khác
            CreateMap<UpdateCommentDTO, Comment>();

            CreateMap<Comment, AddCommentDTO>().ReverseMap();
            CreateMap<CheckIsCommentDTO, AddCommentDTO>().ReverseMap();
            //get
            CreateMap<Comment, GetCommentDTO>()
                .ForMember(dest => dest.DetailProduct, opt => opt.MapFrom(src => src.DetailProduct))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account));

            CreateMap<Account, GetCommentDTO_Account>().ReverseMap();
            CreateMap<DetailProduct, GetCommentDTO_DetailProduc>().ReverseMap();
        }
    }
}
