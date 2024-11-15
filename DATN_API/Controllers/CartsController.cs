using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATN_Core.Entities;
using DATN_Infrastructure.Data;
using AutoMapper;
using DATN_Core.Interface;
using DATN_API.Helper;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using DATN_Core.DTO;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CartsController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-cart")]
        public async Task<ActionResult> Get([FromQuery] Params brandParams)
        {
            var src = await _uow.CartReponsitory.GetAllAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<CartDe>>(src.CartsDTO);
            return Ok(new Pagination<CartDe>(brandParams.Pagesize, brandParams.PageNumber,
                src.totalItems, result));
        }
        [HttpPost("add-cart")]
        public async Task<ActionResult> AddCart([FromBody] CreartCart cartsDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.CartReponsitory.AddAsync(cartsDto);

                    return res ? Ok(cartsDto) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UP-cart/{id}")]
        public async Task<ActionResult> UpCart(int id,UpCart cartsDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.CartReponsitory.UpdateCart(id,cartsDto);

                    return res ? Ok(cartsDto) : BadRequest(res);
                }
                return BadRequest($"Not Found Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete-cart/{id}")]
        public async Task<ActionResult> DeteleCart(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.CartReponsitory.DeleteCart(id);

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
