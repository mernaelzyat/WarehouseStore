using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class TopSellingProductDto
    {
        public string Name { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalSales { get; set; }
    }

}
