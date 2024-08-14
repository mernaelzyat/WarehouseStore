using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class MissingProductsStatisticsDto
    {
        public int CurrentMonthCount { get; set; }
        public int PreviousMonthCount { get; set; }
        public double PercentageChange { get; set; }
    }

}
