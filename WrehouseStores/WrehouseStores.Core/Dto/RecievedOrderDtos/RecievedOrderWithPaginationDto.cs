using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.RecievedOrderDtos
{
    public class RecievedOrderWithPaginationDto
    {
        public PaginationDto Pagination { get; set; }

        public List<RecievedOrderDto> RecievedOrders { get; set; }
    }
}
