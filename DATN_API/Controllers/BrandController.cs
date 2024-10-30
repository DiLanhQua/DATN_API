using AutoMapper;
using DATN_Core.Entities;
using Microsoft.AspNetCore.Http;
using DATN_Core.Interface;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DATN_Core.Sharing;
using DATN_API.Helper;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public BrandController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-brand")]
        public async Task<ActionResult> Get([FromQuery] BrandParams brandParams)
        {
            var src = await _uow.BrandReponsitory.GetAllAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<BrandDTO>>(src.BrandsDTO);
            return Ok(new Pagination<BrandDTO>(brandParams.Pagesize, brandParams.PageNumber,src.totalItems, result));
        }


        [HttpGet("get-brand-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var brand = await _uow.BrandReponsitory.GetAsync(id);
            if (brand == null)
            {
                return BadRequest($"Not found id = [{id}]");


            }
            return Ok(_mapper.Map<Brand, BrandDTO>(brand));
        }

        [HttpPost("add-brand")]
        public async Task<ActionResult> Addbrand([FromForm]CreateBrandDTO brandDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.BrandReponsitory.AddAsync(brandDto);

                    return res ? Ok(brandDto) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-brand-by-id/{id}")]
        public async Task<ActionResult> Updatebrand(int id, UpdateBrandDTO updateBrandDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.BrandReponsitory.UpdateAsync(id, updateBrandDTO );

                    return res ? Ok(updateBrandDTO) : BadRequest(res);
                }
                return BadRequest($"Not Found Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-brand-by-id/{id}")]
        public async Task<ActionResult> Removebrand(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cate = await _uow.BrandReponsitory.GetAsync(id);
                    if (cate != null)
                    {
                        await _uow.BrandReponsitory.DeleteAsync(id);
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
