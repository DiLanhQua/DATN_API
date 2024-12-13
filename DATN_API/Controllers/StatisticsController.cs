using AutoMapper;
using DATN_Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public StatisticsController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }
        [HttpGet("get-all-statistics")]
        public async Task<ActionResult> Get()
        {
            var statistics = await _uow.StatisticsReponsitory.GetStatistics();
            return Ok(new
            {
                TongTienGioHang = statistics.TongTienGioHang,
                Count2 = statistics.Count2,
                TotalAmount1 = statistics.TotalAmount1,
                Count1 = statistics.Count1,
                TongTienGioHangs = statistics.TongTienGioHangs,
                Count = statistics.Count,
                TotalAmount = statistics.TotalAmount,
                Demkh = statistics.Demkh,
                Tongdoanhthu = statistics.Tongdoanhthu,
                Tongdoanhthutheothang = statistics.Tongdoanhthutheothang,
                DemkhChuaXacNhan = statistics.DemkhChuaXacNhan,
                DemkhXacNhan = statistics.DemkhXacNhan,
                Sodonhanghuy = statistics.Sodonhanghuy,
                Sodonhanghuytrongthang = statistics.Sodonhanghuytrongthang
            });
        }
        [HttpGet("add-statistics")]
        public async Task<IActionResult> AddStatistics(string viewType)
        {
            // Kiểm tra giá trị viewType hợp lệ
            if (string.IsNullOrEmpty(viewType) || !(viewType == "day" || viewType == "month" || viewType == "year"))
            {
                return BadRequest("viewType không hợp lệ. Phải là 'day', 'month' hoặc 'year'.");
            }

            try
            {
                var result = await _uow.StatisticsReponsitory.AddStatistics(viewType);

                return Ok(new
                {
                    Labels = result.Labels,
                    Numbers = result.Numbers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi.", details = ex.Message });
            }
        }

    }
}
