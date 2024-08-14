using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ReportDtos
{
    public class SearchReportDto
    {
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? TotalStock { get; set; }
    }

}
