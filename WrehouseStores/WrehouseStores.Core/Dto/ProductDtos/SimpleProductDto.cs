using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ProductDtos
{
        

        public class SimpleProductDto
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public int? Quantity { get; set; }
            public decimal? Weight { get; set; }
            public decimal? Price { get; set; }
            public decimal? SalesTax { get; set; }
            public string Description { get; set; }
            public DateTime? ExpiryDate { get; set; }
       // public string? Notes { get; set; }
    }

    }


