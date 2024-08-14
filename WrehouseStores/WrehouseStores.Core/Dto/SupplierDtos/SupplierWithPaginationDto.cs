using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.SupplierDtos
{
    public class SupplierWithPaginationDto
    {
        public PaginationDto Pagination { get; set; }
        public List<SupplierDto> Suppliers { get; set; }
    }
}
