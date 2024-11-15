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
        }
    }
}
