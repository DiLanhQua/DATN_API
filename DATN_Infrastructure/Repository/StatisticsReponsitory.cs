using AutoMapper;
using DATN_Core.DTO;
using DATN_Core.Interface;
using DATN_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Repository
{
    public class StatisticsReponsitory : IStatisticsReponsitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public StatisticsReponsitory(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<AddStatisticsDTO> AddStatistics(string viewType)
        {
            List<object> data = new List<object>();
            List<string> labels = new List<string>();
            List<decimal?> numbers = new List<decimal?>();

            if (viewType == "day")
            {
                List<Tuple<DateTime?, decimal?>> dayLabels = _context.Orders
                    .Where(gh => gh.StatusOrder == 4)
                    .Select(p => new Tuple<DateTime?, decimal?>(p.TimeOrder, p.Total))
                    .ToList();

                var groupe_contextyDay = dayLabels.GroupBy(d => d.Item1?.ToString("dd/MM/yyyy"))
                    .Select(g => new { Day = g.Key, Total = g.Sum(d => d.Item2) })
                    .ToList();

                labels = groupe_contextyDay.Select(g => g.Day).ToList();
                numbers = groupe_contextyDay.Select(g => g.Total).ToList();
            }
            else if (viewType == "month")
            {
                List<Tuple<DateTime?, decimal?>> monthLabels = _context.Orders
                    .Where(gh => gh.StatusOrder == 4)
                    .Select(p => new Tuple<DateTime?, decimal?>(p.TimeOrder, p.Total))
                    .ToList();

                var groupe_contextyMonth = monthLabels.GroupBy(d => new { d.Item1?.Year, d.Item1?.Month })
                    .Select(g => new { Month = g.Key, Total = g.Sum(d => d.Item2) })
                    .ToList();

                labels = groupe_contextyMonth.Select(g => $"Tháng {g.Month.Month} {g.Month.Year}").ToList();
                numbers = groupe_contextyMonth.Select(g => g.Total).ToList();
            }
            else if (viewType == "year")
            {
                List<Tuple<DateTime?, decimal?>> yearLabels = _context.Orders
                    .Where(gh => gh.StatusOrder == 4)
                    .Select(p => new Tuple<DateTime?, decimal?>(p.TimeOrder, p.Total))
                    .ToList();

                var groupe_contextyYear = yearLabels.GroupBy(d => d.Item1?.Year)
                    .Select(g => new { Year = g.Key, Total = g.Sum(d => d.Item2) })
                    .ToList();

                labels = groupe_contextyYear.Select(g => $"Năm {g.Year}".ToString()).ToList();

                numbers = groupe_contextyYear.Select(g => g.Total).ToList();
            }

            data.Add(labels);
            data.Add(numbers);

            return Task.FromResult(new AddStatisticsDTO
            {
                Labels = labels,
                Numbers = numbers
            });
        }

        public async Task<StatisticsDTO> GetStatistics()
        {
            var gioHangItems = await _context.Orders.ToListAsync();
            decimal? doanhthu = gioHangItems
                .Where(gh => gh.StatusOrder == 4)
                .Sum(gh => gh.Total);
            string tongTienForTrangThaied = doanhthu?.ToString("N0");

            int tongdoanhthu = _context.Orders.Count(item => item.StatusOrder == 3 || item.StatusOrder == 4);

            int tongdoanhthutheothang = _context.Orders.Where(item => item.TimeOrder.Month == DateTime.Now.Month)
                .Count(item => item.StatusOrder == 3 || item.StatusOrder == 4);
            int donhangdanhan = _context.Orders.Count(item => item.StatusOrder == 3);

            int donhangdahoanthanh = _context.Orders.Count(item => item.StatusOrder == 4);
            //đon hàng hủy
            int donhangdahuy = _context.Orders.Count(item => item.StatusOrder == 5);
            int tongdonhangdahuytrongthang = _context.Orders.Where(item => item.TimeOrder.Month == DateTime.Now.Month)
                .Count(item => item.StatusOrder == 5);



            decimal? doanhthutheothang = _context.Orders
                .Where(item => item.StatusOrder == 4 && item.TimeOrder.Month == DateTime.Now.Month)
                .Sum(item => item.Total);
            string forTrangThaiedTotalAmount1 = doanhthutheothang?.ToString("N0");

            decimal? tongTiens = gioHangItems
                .Where(gh => gh.StatusOrder == 3)
                .Sum(gh => gh.Total);
            string tongTienmtt = tongTiens?.ToString("N0");

            int count = _context.Orders.Count(item => item.StatusOrder == 1);
            decimal? totalAmount = _context.Orders
                .Where(item => item.StatusOrder == 1)
                .Sum(item => item.Total);
            string forTrangThaiedTotalAmount = totalAmount?.ToString("N0");

            int demkh = await _context.Accounts.CountAsync(item => item.Role == 2);
            int demkhchuaxacnhan = await _context.Accounts.Where(item => item.Status == 0)
                .CountAsync(item => item.Role == 2);
            int demkhxacnhan = await _context.Accounts.Where(item => item.Status == 1)
                .CountAsync(item => item.Role == 2);
            int demkhKhoa = await _context.Accounts.Where(item => item.Status == 2)
                .CountAsync(item => item.Role == 2);

            // Create and return the StatisticsDTO object
            return new StatisticsDTO
            {
                TongTienGioHang = tongTienForTrangThaied,
                Count2 = donhangdanhan,
                TotalAmount1 = forTrangThaiedTotalAmount1,
                Count1 = donhangdahoanthanh,
                TongTienGioHangs = tongTienmtt,
                Count = count,
                TotalAmount = forTrangThaiedTotalAmount,
                Demkh = demkh,
                Tongdoanhthu = tongdoanhthu,
                Tongdoanhthutheothang = tongdoanhthutheothang,
                DemkhChuaXacNhan = demkhchuaxacnhan,
                DemkhXacNhan = demkhxacnhan,
                Sodonhanghuy = donhangdahuy,
                Sodonhanghuytrongthang = tongdonhangdahuytrongthang
            };
        }

    }
}
