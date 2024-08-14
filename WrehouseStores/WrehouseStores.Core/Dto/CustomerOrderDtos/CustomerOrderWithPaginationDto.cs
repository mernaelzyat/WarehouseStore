using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.CustomerOrderDtos
{
    public class CustomerOrderWithPaginationDto
    {

        public List<CustomerOrdersDto> CustomerOrders { get; set; }
        public PaginationDto? Pagination { get; set; }

    }
}
