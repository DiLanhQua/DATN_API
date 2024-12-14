using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using DATN_Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public LoginController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        [HttpGet("get-login-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var query = await _uow.LoginReponsitory.GetByIdAsync(id);
            if (query == null)
            {
                return BadRequest($"Không tìm thấy thông tin này");
            }
            return Ok(query);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountUser loginAccount)
        {
            if (loginAccount == null || string.IsNullOrEmpty(loginAccount.UserName) || string.IsNullOrEmpty(loginAccount.Password))
            {
                return BadRequest(new { Message = "Tên đăng nhập và mật khẩu không được để trống!" });
            }

            try
            {
                var account = await _uow.LoginReponsitory.Login(loginAccount.UserName, loginAccount.Password);

                if (account == null)
                {
                    return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không chính xác!" });
                }

                // Kiểm tra trạng thái tài khoản
                if (account.Status != 1)
                {
                    return Forbid(new { Message = "Tài khoản của bạn không hoạt động. Vui lòng xác nhận tài khoản bằng Email." });
                }

                return Ok(new
                {
                    Message = "Đăng nhập thành công!",
                    AccountId = account.Id,
                    FullName = account.FullName,
                    Role = account.Role,
                    UserName = account.UserName

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi trong quá trình xử lý!", Error = ex.Message });
            }
        }

        private IActionResult Forbid(object value)
        {
            throw new NotImplementedException();
        }

        [HttpGet("get-all-login")]
        public async Task<IActionResult> GetAll([FromQuery] Params loginParams)
        {
            try
            {
                var result = await _uow.LoginReponsitory.GetAllAsync(loginParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi lấy dữ liệu!", Error = ex.Message });
            }
        }

        //Đăng ký
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var res = await _uow.LoginReponsitory.RegisterAsync(registerDTO);

                    return res ? Ok(registerDTO) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("up-profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromForm] UpProfile updateRequest)
        {
            try
            {
                var updatedAccount = await _uow.LoginReponsitory.UpdateProfileAsync(id, updateRequest);
                if (updatedAccount != null)
                {
                    return Ok(new
                    {
                        Message = "Cập nhật thông tin thành công!",
                        Account = new
                        {
                            Id = updatedAccount.Id,
                            FullName = updatedAccount.FullName,
                            Email = updatedAccount.Email,
                            Phone = updatedAccount.Phone,
                            Address = updatedAccount.Address,
                            Image = updatedAccount.Image
                        }
                    });
                }
                else
                {
                    return BadRequest(new { Message = "Cập nhật không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi trong quá trình cập nhật!", Error = ex.Message });
            }
        }




        [HttpPost("xn-account/{idaccount}")]
        public async Task<ActionResult> XNAccount(int idaccount)
        {
            try
            {
                await _uow.AccountReponsitory.XNAccount(idaccount);

                return Ok(new { Message = "Xác nhận thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
