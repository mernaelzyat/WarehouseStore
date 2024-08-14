using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.ProductDtos
{
    public class ProductsSearchRequestDto
    {
        public string? ProductName { get; set; }
       
        public DateTime? ExpiryDate { get; set; }
    }
}
