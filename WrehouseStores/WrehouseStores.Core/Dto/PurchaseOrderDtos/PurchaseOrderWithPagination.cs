using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.PurchaseOrderDtos
{
    public class PurchaseOrderWithPagination
    {
        public PaginationDto Pagination { get; set; }
        public List<PurchaseOrderDto> PurchaseOrders { get; set; }
    }
}
