using AutoMapper;
using DATN_API.Helper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }

        [HttpGet("get-all-product")]
        public async Task<ActionResult> Get([FromQuery] Params brandParams)
        {
            var src = await _uow.ProductReponsitory.GetAllAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<ProductDTO>>(src.Products); // Sử dụng ProductDTO ở đây
            return Ok(new Pagination<ProductDTO>(brandParams.Pagesize, brandParams.PageNumber, src.TotalItems, result));
        }


        [HttpPost("add-product")]
        public async Task<ActionResult> Addproduct(ProductDTO proDto)
        {
            try
            {
                /* if (ModelState.IsValid)
                 {
                     var res = await _uow.ProductReponsitory.AddAsync(proDto);

                     return res ? Ok(proDto) : BadRequest(res);


                 }
                 return BadRequest();*/

                if (ModelState.IsValid)
                {
                    var res = _mapper.Map<Product>(proDto);
                    await _uow.ProductReponsitory.AddAsync(res);
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UP-product/{id}")]
        public async Task<ActionResult> Upproduct(int id, ProductDTO proDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.ProductReponsitory.Updateproduct(id, proDto);

                    return res ? Ok(proDto) : BadRequest(res);
                }
                return BadRequest($"Not Found Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete-product/{id}")]
        public async Task<ActionResult> Deteleproduct(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.ProductReponsitory.Deleteproduct(id);

                    return res ? Ok("Đã xóa") : BadRequest(res);
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
