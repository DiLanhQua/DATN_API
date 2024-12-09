using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public BlogController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-blog")]
        public async Task<ActionResult> Get([FromQuery] Params brandParams)
        {
            var src = await _uow.BlogReponsitory.GetAllAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<BlogDTO>>(src.BlogsDTO);
            return Ok(new Pagination<BlogDTO>(brandParams.Pagesize, brandParams.PageNumber, src.totalItems, result));
        }

        [HttpGet("blog-id/{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            try
            {
                BlogDTO respose = await _uow.BlogReponsitory.GetBlogByIdAsync(id);

                return Ok(respose);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("image-blog/{idBlog}")]
        public async Task<IActionResult> GetImageByIdBlog(int idBlog)
        {
            try
            {
                List<ImageBlogDtos> respose = await _uow.BlogReponsitory.GetImageByIdBlog(idBlog);

                return Ok(respose);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-blog")]
        public async Task<ActionResult> AddBlog([FromBody] CreateBlogDTO mediaDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.BlogReponsitory.AddAsync(mediaDTO);

                    return res ? Ok(mediaDTO) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-blog-by-id/{id}")]
        public async Task<ActionResult> UpdateMedia(int id, CreateBlogDTO updateMediaDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.BlogReponsitory.UpdateAsync(id, updateMediaDTO);

                    return res ? Ok(updateMediaDTO) : BadRequest(res);
                }
                return BadRequest($"Not Found Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("is-primary/{idImage}")]
        public async Task<IActionResult> IsPrimaryImageBlog(int idImage)
        {
            try
            {
                bool response = await _uow.BlogReponsitory.IsPrimaryBlog(idImage);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-image/{idImage}")]
        public async Task<IActionResult> DeleteImageById(int idImage)
        {
            try
            {
                bool response = await _uow.BlogReponsitory.DeleteImageBlog(idImage);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-blog-by-id/{id}")]
        public async Task<ActionResult> RemoveMedia(int id)
        {
            try
            {
                bool response = await _uow.BlogReponsitory.DeleteBlogById(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
