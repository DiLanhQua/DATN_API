using AutoMapper;
using DATN_Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MediasController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet("get-medias/{idproduct}")]
        public async Task<ActionResult> Medias(int idproduct)
        {
            var result = await _uow.MediaReponsitory.GetMedia(idproduct);
<<<<<<< Updated upstream
            if (result == null )
=======
            if (result == null)
>>>>>>> Stashed changes
            {
                return NotFound(new { message = "No detail products found for the given product ID." });
            }
            return Ok(result);
        }
<<<<<<< Updated upstream
=======
        
>>>>>>> Stashed changes
    }
}
