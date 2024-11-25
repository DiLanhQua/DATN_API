using AutoMapper;
using DATN_API.Helper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

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
            var result = _mapper.Map<IReadOnlyList<ProductDEDTO>>(src.Products); // Sử dụng ProductDTO ở đây
            return Ok(new Pagination<ProductDEDTO>(brandParams.Pagesize, brandParams.PageNumber, src.TotalItems, result));
        }
        [HttpGet("get-product/{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var result = await _uow.ProductReponsitory.GetProduct(id);
            if (result == null)
            {
                return NotFound(new { message = "No detail products found for the given product ID." });
            }
            return Ok(result);
        }

        [HttpPost("add-product")]
        public async Task<ActionResult> Addproduct([FromBody] ProductDTO proDto)
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
                    var res = await _uow.ProductReponsitory.AddProduct(proDto);

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
                    var res = await _uow.ProductReponsitory.UpdateProduct(id, proDto);

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
