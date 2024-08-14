using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.CustomerOrderDtos
{
    public class ReadyOrdersWithPaginationDto
    {
        public PaginationDto Pagination { get; set; }
        public List<ReadyOrdersDto> ReadyOrders { get; set; }
    }
}
