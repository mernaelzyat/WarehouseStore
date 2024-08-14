using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Dto.CustomerOrderDtos
{
    public class AddReadyOrderDto
    {
        public int CustomerOrderId { get; set; }
        public List<ProductUpdateDto> Products { get; set; }
        public string DepartmentName { get; set; } // Added
        public string StatusName { get; set; } // Added
    }
}
