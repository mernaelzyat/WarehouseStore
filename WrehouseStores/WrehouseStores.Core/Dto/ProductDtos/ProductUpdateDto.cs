using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ProductDtos
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public decimal? Weight { get; set; }
        public string? Notes { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalesTax { get; set; }
        public string? Description { get; set; }

        public int? TotalStock { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<ProductDatesDto>? ProductDatesDto { get; set; }
    }
}
