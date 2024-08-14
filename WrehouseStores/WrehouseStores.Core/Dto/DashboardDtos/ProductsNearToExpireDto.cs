using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class ProductsNearToExpireDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public int TotalCount { get; set; }
    }
}
