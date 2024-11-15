using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using DATN_Infrastructure.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ImageController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpPost("add-Image")]
        public async Task<ActionResult> AddImage([FromForm] CreateImageDTO imgDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.InImageReponsitory.AddAsync(imgDto);

                    return res ? Ok(imgDto) : BadRequest(res);
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
