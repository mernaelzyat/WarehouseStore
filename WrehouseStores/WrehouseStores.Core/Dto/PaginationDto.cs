using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto
{
    public class PaginationDto
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? Total { get; set; }
        public bool? HasNextPage { get; set; }
        public bool? HasPreviousPage { get; set; }
    }
}
