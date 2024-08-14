using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.CategoryDtos;

namespace WarehouseStores.Core.Dto.ProductDtos
{
    public class ProductAddDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public string? Notes { get; set; }
        public decimal? SalesTax { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public decimal Weight { get; set; }

        public int? TotalStock { get; set; }
        public ICollection<ProductDatesDto>? ProductDatesDto { get; set; }

        public SimpleCategoryDto? Category { get; set; }
        public decimal? Price { get; set; }
    }
}
