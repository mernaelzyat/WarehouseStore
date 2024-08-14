using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.CategoryDtos;

namespace WarehouseStores.Core.Dto.ProductDtos
{
    public class ProductWithStorageDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public List<ProductDatesDto> ProductDatesDto { get; set; }
        public int? Quantity { get; set; }
        public SimpleCategoryDto Category { get; set; }
        public decimal? Weight { get; set; }
        public string StorageName { get; set; }
    }
}
