using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class CustomerOrdersProducts
    {
        public int Id { get; set; }
        [ForeignKey("CustomerOrderId")]
        public int? CustomerOrderId { get; set; }
        public CustomerOrders? CustomerOrder { get; set; }

        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        public Products? Product { get; set; }

        public int? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public string? Unit { get; set; }
    }
}
