using AutoMapper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data.DTO;
using DATN_Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-category")]
        public async Task<ActionResult> Get()
        {
            var all_cate = await _uow.CategoryReponsitory.GetAllAsync();
            if(all_cate != null)
            {
                var all_cate_list = all_cate.ToList();

                var res = _mapper.Map<IReadOnlyList<Category>,IReadOnlyList<ListCategoryDTO>>(all_cate_list);

                return Ok(res);
            }
            return BadRequest("Not Found");
        }
        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var category = await _uow.CategoryReponsitory.GetAsync(id);
            if (category == null)
            {
                return BadRequest($"Not found id = [{id}]");
                

            }
            return Ok(_mapper.Map<Category,ListCategoryDTO>(category));
        }
        [HttpPost("add-category")]
        public async Task<ActionResult> AddCategory(CategoryDTO categoryDto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var res = _mapper.Map<Category>(categoryDto);                 
                    await _uow.CategoryReponsitory.AddAsync(res);
                    return Ok(res);
                }
                return BadRequest();
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-category-by-id/{id}")]
        public async Task<ActionResult> UpdateCategory(int id,CategoryDTO categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cate = await _uow.CategoryReponsitory.GetAsync(id);
                    if (cate != null)
                    {
                        //cate.CategoryName = categoryDto.CategoryName;
                        //cate.Image = categoryDto.Image;
                        _mapper.Map(categoryDto, cate);
                    }
                    await _uow.CategoryReponsitory.UpdateAsync(id, cate);
                    return Ok(cate);
                }
                return BadRequest($"Not Found Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> RemoveCategory(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cate = await _uow.CategoryReponsitory.GetAsync(id);
                    if (cate != null)
                    {
                      await _uow.CategoryReponsitory.DeleteAsync(id);
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
