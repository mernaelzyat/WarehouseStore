using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Models;

namespace WarehouseStores.Core.Dto.CategoryDtos
{
    public class CategoryDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalProducts { get; set; }
        public bool? HasNextPage { get; set; }
        public bool? HasPreviousPage { get; set; }
        public List<ProductDto>? Products { get; set; }

    }
}
