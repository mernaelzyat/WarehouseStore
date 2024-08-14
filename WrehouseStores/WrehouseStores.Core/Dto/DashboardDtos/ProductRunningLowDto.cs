using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class ProductRunningLowDto
    {
        public string Name { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalStock { get; set; }
        public double Percentage { get; set; }
    }
}
