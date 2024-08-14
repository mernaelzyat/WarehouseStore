using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ReportDtos
{
    public class StockReportDto
    {
        public string ProductName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int? Quantity { get; set; } // currentStock
        public int? TotalStock { get; set; }
    }
}
