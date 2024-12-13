using DATN_Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface IStatisticsReponsitory
    {
        Task<StatisticsDTO> GetStatistics();
        Task<AddStatisticsDTO> AddStatistics(string viewType);

    }
}
