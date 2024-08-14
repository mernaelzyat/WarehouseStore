using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.ReportDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{

    public class ReportController : BaseApiController
    {

        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("GetStockReportsOverTime")]
        public async Task<ActionResult<IEnumerable<StockReportDto>>> GetStockReportsOverTime([FromQuery] string productName)
        {
            var stockReports = await _reportRepository.GetStockReportsOverTimeAsync(productName);
            return Ok(stockReports);
        }

        [HttpGet("GetCurrentStockStatus")]
        public async Task<ActionResult<IEnumerable<StockStatusDto>>> GetCurrentStockStatus()
        {
            var currentStockStatus = await _reportRepository.GetCurrentStockStatusAsync();
            return Ok(currentStockStatus);
        }

        [HttpGet("SearchReport")]
        public async Task<IActionResult> SearchReportAsync(string productName)
        {
            var result = await _reportRepository.SearchReportAsync(productName);
            return Ok(result);
        }
    }
}
