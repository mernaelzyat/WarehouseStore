using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.BillDtos
{
    public class ProductionBillWithPaginationDto
    {
        public PaginationDto Pagination { get; set; }
        public List<ProductionBillDto> ProductionBills { get; set; }
    }
}
