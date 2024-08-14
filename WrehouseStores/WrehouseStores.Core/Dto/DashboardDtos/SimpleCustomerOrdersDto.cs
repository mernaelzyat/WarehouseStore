using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class SimpleCustomerOrdersDto
    {
        public int? Id { get; set; }
        public List<SimpleProductDto> Products { get; set; }
        public string Status { get; set; }
        public DateTime? DateOfOrder { get; set; }
    }
}
