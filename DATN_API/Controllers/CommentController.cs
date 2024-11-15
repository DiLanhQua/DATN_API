using AutoMapper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data.DTO;
using DATN_Core.Sharing;
using Microsoft.AspNetCore.Mvc;
using DATN_API.Helper;
using System.Threading.Tasks;
using DATN_Core.DTO;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CommentController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("get-all-comments")]
        public async Task<ActionResult> GetAll([FromQuery] BrandParams commentParams)
        {
            var src = await _uow.CommentRepository.GetAllAsync(commentParams);
            var result = _mapper.Map<IReadOnlyList<CommentDTO>>(src.CommentsDTO);
            return Ok(new Pagination<CommentDTO>(commentParams.Pagesize, commentParams.PageNumber, src.totalItems, result));
        }

        [HttpGet("get-comment-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var comment = await _uow.CommentRepository.GetAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with id [{id}] not found.");
            }
            return Ok(_mapper.Map<Comment, CommentDTO>(comment));
        }

        [HttpPost("add-comment")]
        public async Task<ActionResult> AddComment([FromBody] CreateCommentDTO commentDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.CommentRepository.AddAsync(commentDto);
                    return res ? Ok(commentDto) : BadRequest("Failed to add comment.");
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-comment-by-id/{id}")]
        public async Task<ActionResult> UpdateComment(int id, [FromBody] UpdateCommentDTO updateCommentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.CommentRepository.UpdateAsync(id, updateCommentDTO);
                    return res ? Ok(updateCommentDTO) : BadRequest("Failed to update comment.");
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-comment-by-id/{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            try
            {
                var comment = await _uow.CommentRepository.GetAsync(id);
                if (comment != null)
                {
                    await _uow.CommentRepository.DeleteAsync(id);
                    return Ok($"Comment with id [{id}] deleted successfully.");
                }
                return NotFound($"Comment with id [{id}] not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
