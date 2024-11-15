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
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public AccountController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-account")]
        public async Task<ActionResult> Get([FromQuery] Params brandParams)
        {
            var src = await _uow.AccountReponsitory.GetAllAsync(brandParams);
            var result = _mapper.Map<IReadOnlyList<AccountDTO>>(src.AccountsDTO);
            return Ok(new Pagination<AccountDTO>(brandParams.Pagesize, brandParams.PageNumber,
                src.totalItems, result));
        }
        [HttpGet("getDe-account/{idaccount}")]
        public async Task<AccountCT> GetDeAccount(int idaccount)
        {
                var result = await _uow.AccountReponsitory.GetDeAccount(idaccount);

                return result;
        }
        [HttpPost("add-account")]
        public async Task<ActionResult> AddAccount([FromForm]CreartAccount accountDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uow.AccountReponsitory.AddAccount(accountDTO);

                    return res ? Ok(accountDTO) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("up-account/{idaccount}")]
        public async Task<ActionResult> UpAccount(int idaccount,[FromForm] UpAccount upAccount)
        {
            try
            {
                if (idaccount != upAccount.AccountId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    var res = await _uow.AccountReponsitory.UpAccount(idaccount, upAccount);

                    return res ? Ok(upAccount) : BadRequest(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
