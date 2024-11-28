using AutoMapper;
using DATN_Core.Entities;
using DATN_Core.Interface;
using DATN_Infrastructure.Data.DTO;
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
            if (result == null)
            {
                return NotFound(new { message = "No detail products found for the given product ID." });
            }
            return Ok(result);
        }
        [HttpGet("get-all-medias/{idproduct}")]
        public async Task<ActionResult> ALLMedias(int idproduct)
        {
            var result = await _uow.MediaReponsitory.GetALLMedia(idproduct);
            if (result == null)
            {
                return NotFound(new { message = "No detail products found for the given product ID." });
            }
            return Ok(result);
        }
        [HttpPost("add-medias/{productId}")]
        public async Task<ActionResult> ADDMedias(int productId, MediaADD mediaAdd)
        {
            try
            {
                // Kiểm tra tính hợp lệ của ModelState
                if (ModelState.IsValid)
                {
                    var res = await _uow.MediaReponsitory.AddMedia(productId,mediaAdd);

                    // Trả về kết quả theo kiểu ActionResult
                    if (res)
                    {
                        return Ok(new { message = "Media đã được thêm thành công!" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Có lỗi xảy ra khi thêm media." });
                    }
                }
                else
                {
                    // Trả về thông tin lỗi nếu ModelState không hợp lệ
                    return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ.", errors = ModelState });
                }
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi chi tiết khi có ngoại lệ
                return BadRequest(new { message = "Có lỗi xảy ra", details = ex.Message });
            }
        }
        [HttpDelete("delete-medias/{mediaId}")]
        public async Task<ActionResult> DeMedias(int mediaId)
        {
            try
            {
                    var res = await _uow.MediaReponsitory.DeleteMedia(mediaId);

                return Ok();
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi chi tiết khi có ngoại lệ
                return BadRequest(new { message = "Có lỗi xảy ra", details = ex.Message });
            }
        }
        [HttpPut("setPrimaryImage")]
        public async Task<IActionResult> SetPrimaryImage(int productId, int imageId)
        {
            try
            {
                var result = await  _uow.MediaReponsitory.SetPrimaryImage(productId, imageId);
                if (result)
                {
                    return Ok(new { message = "Ảnh chính đã được cập nhật thành công." });
                }
                return NotFound(new { message = "Không tìm thấy ảnh để cập nhật." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi", error = ex.Message });
            }
        }

        [HttpPut("changeImage")]
        public async Task<IActionResult> ChangeImage(int productId, int imageId, [FromBody] MediaADD mediaAdd)
        {
            try
            {
                var result = await _uow.MediaReponsitory.ChangeImage(productId, imageId, mediaAdd);
                if (result)
                {
                    return Ok(new { message = "Ảnh đã được thay đổi thành công." });
                }
                return BadRequest(new { message = "Không thể thay đổi ảnh." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi", error = ex.Message });
            }
        }
    }
}
