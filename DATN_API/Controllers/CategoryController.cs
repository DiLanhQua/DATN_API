using AutoMapper;
using DATN_API.Helper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
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
        public async Task<ActionResult> Get([FromQuery] Params brandParams)
        {
            var src = await _uow.CategoryReponsitory.GetAllAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<CataDTO>>(src.cataDTOs);
            return Ok(new Pagination<CataDTO>(brandParams.Pagesize, brandParams.PageNumber, src.totalItems, result));
        }


        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var ct = await _uow.CategoryReponsitory.GetAsync(id);
            if (ct == null)
            {
                return BadRequest($"Not found id = [{id}]");


            }
            return Ok(_mapper.Map<Category, CataDTO>(ct));
        }

        [HttpPost("add-catagory")]
        public async Task<ActionResult> Addbrand([FromForm] CreateCatagoryDTO catagoryDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.CategoryReponsitory.AddAsync(catagoryDTO);

                    return res ? Ok(catagoryDTO) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-category-by-id/{id}")]
        public async Task<ActionResult> Updatebrand([FromForm] UpdateCatagoryDTO updateCatagoryDTO, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.CategoryReponsitory.UpdateAsync(id, updateCatagoryDTO);

                    return res ? Ok(updateCatagoryDTO) : BadRequest(res);
                }
                return BadRequest($"Not Found Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> Removebrand(int id)
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
