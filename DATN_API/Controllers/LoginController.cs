﻿using AutoMapper;
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
        [HttpPut("up-profile/{accountId}")]
        public async Task<IActionResult> UpdateProfile(int accountId, [FromForm] UpProfile updateRequest)
        {
            try
            {
                var isUpdated = await _uow.LoginReponsitory.UpdateProfileAsync(accountId, updateRequest);
                if (isUpdated)
                {
                    return Ok(new { message = "Cập nhật thông tin thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Cập nhật không thành công!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
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
