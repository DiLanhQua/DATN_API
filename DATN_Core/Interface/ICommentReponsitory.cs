using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface ICommentRepository : IGenericeReponsitory<Comment>
    {
        Task<ReturnCommentDTO> GetAllAsync(Params commentParams);
        Task<bool> AddAsync(CreateCommentDTO commentDTO);
        Task<bool> UpdateAsync(int id, UpdateCommentDTO commentDTO);
        Task<bool> CheckIsComment(CheckIsCommentDTO dto);
        Task<bool> AddComment(AddCommentDTO commentDTO);
        Task<(List<GetCommentDTO> Comments, bool HasMore)> GetCommentByProductId(int id, int pageNumber);
    }
}
