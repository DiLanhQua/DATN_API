using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailOrderController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DetailOrderController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-detailorder")]
        public async Task<ActionResult> Get()
        {
            var src = await _uow.DetailOrderReponsitory.GetAllAsync();
            return Ok(src); // Directly return the ReturnOrder object
        }

        [HttpPost("add-detailorder")]
        public async Task<ActionResult> AddDetailOrder([FromBody] CreateDetailOrder orderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.DetailOrderReponsitory.AddAsync(orderDTO);

                    return res ? Ok(orderDTO) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
