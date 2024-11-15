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


        [HttpGet("get-blog-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var brand = await _uow.BlogReponsitory.GetAsync(id);
            if (brand == null)
            {
                return BadRequest($"Not found id = [{id}]");


            }
            return Ok(_mapper.Map<Blog, BlogDTO>(brand));
        }

        [HttpPost("add-blog")]
        public async Task<ActionResult> AddBlog([FromForm] CreateBlogDTO mediaDTO)
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
        public async Task<ActionResult> UpdateMedia(int id, UpdateDTO updateMediaDTO)
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
        [HttpDelete("delete-blog-by-id/{id}")]
        public async Task<ActionResult> RemoveMedia(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cate = await _uow.BlogReponsitory.GetAsync(id);
                    if (cate != null)
                    {
                        await _uow.BlogReponsitory.DeleteAsync(id);
                    }
                    return Ok(cate);
                }
                return BadRequest($"Not Found Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
