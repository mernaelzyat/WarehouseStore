using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.StatusDtos;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class CustomerOrderStatusSearchDto
    {
        public int? Id { get; set; }
        public List<SimpleProductDto> Products { get; set; }
        //public DateTime DateOfOrder { get; set; }
       // public string Status { get; set; }
        public StatusDto? StatusDto { get; set; }
        public DateTime? Time { get; set; }
    }
}
