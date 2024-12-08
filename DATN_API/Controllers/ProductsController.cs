using AutoMapper;
using DATN_API.Helper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUserPage([FromQuery] Params brandParams)
        {
            var src = await _uow.ProductReponsitory.GetAllUserAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<ProductsUserDtos>>(src.Products); // Sử dụng ProductDTO ở đây
            return Ok(new Pagination<ProductsUserDtos>(brandParams.Pagesize, brandParams.PageNumber, src.TotalItems, result));
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

        [HttpGet("hot-sale-product")]
        public async Task<IActionResult> GetHotSaleProducts()
        {
            try
            {
                List<ProductsHotSaleDtos> response = await _uow.ProductReponsitory.GetProductsHotSale();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-product")]
        public async Task<ActionResult> AddProduct([FromBody] ProductDTO proDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.ProductReponsitory.AddProduct(proDto);

                    if (res)
                    {
                        return Ok(new { message = "Sản phẩm đã được thêm thành công!" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Có lỗi xảy ra khi thêm sản phẩm." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ.", errors = ModelState });
                }
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi chi tiết khi có ngoại lệ
                return BadRequest(new { message = "Có lỗi xảy ra", details = ex.Message });
            }
        }



        [HttpPut("UP-product/{id}")]
        public async Task<ActionResult> Upproduct(int id, [FromForm] ProductUPDTO proDto)
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
