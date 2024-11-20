using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_API.Controllers
{




    [Route("api/[controller]")]
    [ApiController]
    public class DetailProductController : ControllerBase
    {


        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DetailProductController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("get-all-detailproduct")]
        public async Task<ActionResult> Get([FromQuery] Params brandParams)
        {
            var src = await _uow.DetailProductReponsitory.GetAllAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<ProductDetailDTO>>(src.Productdetail); // Sử dụng ProductDTO ở đây
            return Ok(new Pagination<ProductDetailDTO>(brandParams.Pagesize, brandParams.PageNumber, src.TotalItems, result));


        }
        
        [HttpGet("get-detailproduct/{idproduct}")]
        public async Task<ActionResult> GetProductDetail(int idproduct)
        {
            var result = await _uow.DetailProductReponsitory.GetDetailProduct(idproduct);
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No detail products found for the given product ID." });
            }
            return Ok(result);
        }
        [HttpPost("add-detailproduct")]
        public async Task<ActionResult> AddDetailProduct(ProductDetailDTO productDetailDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = _mapper.Map<DetailProduct>(productDetailDTO);
                    await _uow.DetailProductReponsitory.AddAsync(res);
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("update-detailproduct-by-id/{id}")]
        public async Task<ActionResult> UpdateDetailProduct(int id, ProductDetailDTO productDetailDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.DetailProductReponsitory.UpdateAsync(id, productDetailDTO);
                    return res ? Ok(productDetailDTO) : BadRequest(res);
                }
                return BadRequest($"Không tìm thấy id: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
