using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ColorController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("get-all-color")]
        public async Task<ActionResult> Get([FromQuery] Params ColorParams)
        {

            var src = await _uow.ColorReponsitory.GetAllAsync(ColorParams);
            var result = _mapper.Map<IReadOnlyList<ColorDTO>>(src.ColorList);
            return Ok(new Pagination<ColorDTO>(ColorParams.Pagesize, ColorParams.PageNumber, src.TotalItems, result));

        }

        [HttpPost("add-color")]
        public async Task<ActionResult> AddColor(ColorAdd colorDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res =  _mapper.Map<Color>(colorDto);
                    await _uow.ColorReponsitory.AddAsync(res);
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("up-color/{id}")]
        public async Task<ActionResult> UpColor(int id, ColorDTO colorDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.ColorReponsitory.UpdateColor(id, colorDto);
                    return res ? Ok(colorDto) : BadRequest();
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
