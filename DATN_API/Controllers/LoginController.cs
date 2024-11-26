using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using DATN_Core.Sharing;
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
        public async Task<ActionResult> Register( RegisterDTO registerDTO)
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
    }
}
