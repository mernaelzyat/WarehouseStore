using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ReportDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<StockReportDto>> GetStockReportsOverTimeAsync(string productName = null);
        Task<IEnumerable<StockStatusDto>> GetCurrentStockStatusAsync();
        Task<IEnumerable<SearchReportDto>> SearchReportAsync(string productName);
    }
}
