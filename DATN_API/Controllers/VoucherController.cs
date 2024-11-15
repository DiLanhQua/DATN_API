using DATN_API.Helper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using DATN_Core.Sharing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public VoucherController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // Lấy danh sách tất cả các voucher
        [HttpGet("get-all-vouchers")]
        public async Task<ActionResult> GetAllVouchers([FromQuery] Params voucherParams)
        {
            try
            {
                var vouchers = await _uow.VoucherRepository.GetAllAsync(voucherParams);
                return Ok(new Pagination<VoucherDTO>(
                    voucherParams.Pagesize,
                    voucherParams.PageNumber,
                    vouchers.TotalItems,
                    vouchers.Vouchers));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Lấy thông tin voucher theo ID
        [HttpGet("get-voucher-by-id/{id}")]
        public async Task<ActionResult> GetVoucherById(int id)
        {
            try
            {
                var voucher = await _uow.VoucherRepository.GetAsync(id);
                if (voucher == null)
                {
                    return NotFound($"Voucher with id [{id}] not found.");
                }
                return Ok(voucher);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Thêm mới một voucher
        [HttpPost("add-voucher")]
        public async Task<ActionResult> AddVoucher([FromBody] CreateVoucherDTO createVoucherDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _uow.VoucherRepository.AddAsync(createVoucherDTO);
                return result ? Ok("Voucher added successfully.") : BadRequest("Failed to add voucher.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Cập nhật thông tin voucher
        [HttpPut("update-voucher/{id}")]
        public async Task<ActionResult> UpdateVoucher(int id, [FromBody] UpdateVoucherDTO updateVoucherDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _uow.VoucherRepository.UpdateAsync(id, updateVoucherDTO);
                return result ? Ok("Voucher updated successfully.") : NotFound($"Voucher with id [{id}] not found.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Xóa voucher theo ID
        //[HttpDelete("delete-voucher/{id}")]
        //public async Task<ActionResult> DeleteVoucher(int id)
        //{
        //    try
        //    {
        //        var voucher = await _uow.VoucherRepository.GetAsync(id);
        //        if (voucher == null)
        //        {
        //            return NotFound($"Voucher with id [{id}] not found.");
        //        }

        //        var result = await _uow.VoucherRepository.DeleteAsync(id);
        //        return result ? Ok($"Voucher with id [{id}] deleted successfully.") : BadRequest("Failed to delete voucher.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error: {ex.Message}");
        //    }
        //}
    }
}
