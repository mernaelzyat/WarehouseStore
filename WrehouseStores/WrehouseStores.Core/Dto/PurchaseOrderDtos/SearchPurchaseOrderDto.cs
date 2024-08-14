using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.PurchaseOrderDtos
{
    public class SearchPurchaseOrderDto
    {
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
    }

}
