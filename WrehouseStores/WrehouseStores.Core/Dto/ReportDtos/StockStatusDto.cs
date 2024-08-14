using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ReportDtos
{
    public class StockStatusDto
    {
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int? Quantity { get; set; }
        public string Status { get; set; }
    }
}
