using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Entities;
using DATN_Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAddressController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DeliveryAddressController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }

        [HttpPost("{idOrder}")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDeliveryDtos modal, int idOrder)
        {
            try
            {
                DeliveryAddress res = await _uow.DeliveryAddressRespository.CreateAsync(modal, idOrder);
                
                await _uow.OrderReponsitory.UpdateQuantityProducts(idOrder, true);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
