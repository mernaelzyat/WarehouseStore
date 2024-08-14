using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.BillDtos
{
    public class SalesSupplyFormWithPaginationDto
    {
        public PaginationDto Pagination { get; set; }
        public List<SalesSupplyFormDto> SalesSupplyForms { get; set; }
    }
}
