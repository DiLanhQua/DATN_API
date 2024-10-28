using AutoMapper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> Get()
        {
            var all_cate = await _uow.BrandReponsitory.GetAllAsync();
            if (all_cate != null)
            {
                var all_cate_list = all_cate.ToList();

                var res = _mapper.Map<IReadOnlyList<Brand>, IReadOnlyList<ListBrandDTO>>(all_cate_list);

                return Ok(res);
            }
            return BadRequest("Not Found");
        }


        [HttpGet("get-brand-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var brand = await _uow.BrandReponsitory.GetAsync(id);
            if (brand == null)
            {
                return BadRequest($"Not found id = [{id}]");


            }
            return Ok(_mapper.Map<Brand, ListBrandDTO>(brand));
        }

        [HttpGet("get-brand-by-id/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var brand = await _uow.BrandReponsitory.GetAsync(id);
            if (brand == null)
            {
                return BadRequest($"Not found id = [{id}]");


            }
            return Ok(_mapper.Map<Brand, ListBrandDTO>(brand));
        }
        [HttpPost("add-brand")]
        public async Task<ActionResult> Addbrand(BrandDTO brandDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = _mapper.Map<Brand>(brandDto);
                    await _uow.BrandReponsitory.AddAsync(res);
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-brand-by-id/{id}")]
        public async Task<ActionResult> Updatebrand(int id, BrandDTO brandDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cate = await _uow.BrandReponsitory.GetAsync(id);
                    if (cate != null)
                    {
                        //cate.brandName = brandDto.brandName;
                        //cate.Image = brandDto.Image;
                        _mapper.Map(brandDto, cate);
                    }
                    await _uow.BrandReponsitory.UpdateAsync(id, cate);
                    return Ok(cate);
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
