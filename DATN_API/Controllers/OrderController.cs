using AutoMapper;
using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Entities;
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

        [HttpGet("get-by-user/{idUser}")]
        public async Task<IActionResult> GetByIdUser(int idUser)
        {
            List<OrderUserDtos> res = await _uow.OrderReponsitory.GetOrderByIdUser(idUser);

            return Ok(res);
        }

        [HttpPost("add-order")]
        public async Task<ActionResult> AddOrder([FromBody] CreateOrder orderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Gọi phương thức AddAsync và nhận OrderId
                    var orderId = await _uow.OrderReponsitory.AddAsync(orderDTO);

                    if (orderId > 0)
                    {
                        if (orderDTO.VoucherId.HasValue)
                        {
                            await _uow.VoucherRepository.DeleteQuantilyVoucher(orderDTO.VoucherId.Value);
                        }

                        // Trả về kết quả thành công với OrderId
                        return Ok(new
                        {
                            Success = true,
                            Message = "Order đã được tạo thành công.",
                            OrderId = orderId
                        });
                    }
                    else
                    {
                        // Trả về lỗi nếu không tạo được Order
                        return BadRequest(new
                        {
                            Success = false,
                            Message = "Không thể tạo Order."
                        });
                    }
                }
                return BadRequest(new
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xử lý yêu cầu.",
                    Error = ex.Message
                });
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
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.OrderReponsitory.GetOrderById(id);
                    if (res != null)
                    {
                        return Ok(res);  // Return 200 OK if order is found
                    }
                    else
                    {
                        return NotFound($"Order with Id [{id}] not found.");  // Return 404 if order not found
                    }
                }
                return BadRequest("Model state is invalid.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  // Return the exception message in case of error
            }
        }

    }
}
