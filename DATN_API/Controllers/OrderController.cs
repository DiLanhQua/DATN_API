using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-order")]
        public async Task<ActionResult> Get()
        {
            var src = await _uow.OrderReponsitory.GetAllAsync();
            return Ok(src); // Directly return the ReturnOrder object
        }

        [HttpPost("add-order")]
        public async Task<ActionResult> AddOrder([FromBody] CreateOrder orderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.OrderReponsitory.AddAsync(orderDTO);

                    return res ? Ok(orderDTO) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UP-order/{id}")]
        public async Task<ActionResult> UpOrder(int id, UpdateOrder orderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.OrderReponsitory.UpdateOrder(id, orderDTO);

                    return res ? Ok(orderDTO) : BadRequest(res);
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
