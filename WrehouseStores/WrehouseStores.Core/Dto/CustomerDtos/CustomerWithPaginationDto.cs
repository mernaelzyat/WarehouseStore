using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.CustomerDtos
{
    public class CustomerWithPaginationDto
    {
        public List<CustomerDto> Customers { get; set; }
        public PaginationDto? Pagination { get; set; }
        //public int? PageNumber { get; set; }
        //public int? PageSize { get; set; }
        //public int? TotalProducts { get; set; }
        //public bool? HasNextPage { get; set; }
        //public bool? HasPreviousPage { get; set; }
    }
}
