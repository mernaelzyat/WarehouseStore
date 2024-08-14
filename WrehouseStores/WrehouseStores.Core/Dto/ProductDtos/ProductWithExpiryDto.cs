using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ProductDtos
{
    public class ProductWithExpiryDto
    {
        //[Required]
        public int? Id { get; set; }
        //  [Required]
        public string? Name { get; set; }
        // [Required]
        public string? Unit { get; set; }
        // [Required]
        public int? Quantity { get; set; }
        // [Required]
        public float Weight { get; set; }
        //  [Required]
        public decimal? Price { get; set; }
        // [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
