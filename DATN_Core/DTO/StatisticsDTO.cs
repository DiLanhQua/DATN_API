using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
    public class StatisticsDTO
    {
        public string TongTienGioHang { get; set; }
        public int Count2 { get; set; }
        public string TotalAmount1 { get; set; }
        public int Count1 { get; set; }
        public string TongTienGioHangs { get; set; }
        public int Count { get; set; }
        public string TotalAmount { get; set; }
        public int Demkh { get; set; }
        public int Tongdoanhthu { get; set; }
        public int Tongdoanhthutheothang { get; set; }
        public int DemkhChuaXacNhan { get; set; }
        public int DemkhXacNhan { get; set; }
        public int Sodonhanghuy {  get; set; }
        public int Sodonhanghuytrongthang {  get; set; }
    }
    public class AddStatisticsDTO
    {
        public List<string> Labels { get; set; }
        public List<decimal?> Numbers { get; set; }
    }

}
