using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ProductDtos
{
    public class ProductWithPaginationDto
    {
        public List<ProductDto> Products { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
