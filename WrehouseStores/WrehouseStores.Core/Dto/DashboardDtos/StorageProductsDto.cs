using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class StorageProductsDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
       
        public int? Quantity { get; set; }
        public int? TotalStock { get; set; }
        public string StorageName { get; set; }
    }
}
