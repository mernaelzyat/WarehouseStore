using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Products
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public decimal? SalesTax { get; set; }
        public string? Description { get; set; }

        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; } // Fk
        public Categories? Category { get; set; } // Navigational property

        public string? Unit { get; set; }
        public decimal? Weight { get; set; }
        public string? Notes { get; set; }
        public int? Quantity { get; set; } // currentStock
       
        public int? TotalStock { get; set; }
        public decimal Price { get; set; }

        public ICollection<ProductDates>? ProductDates { get; set; }
       
        public ICollection<ReadyOrdersProducts> ReadyOrderProducts { get; set; }
        public ICollection<ReceivedOrders>? ReceivedOrders { get; set; }
        public ICollection<PurchaseOrderProducts>? PurchaseOrderProducts { get; set; }
        public ICollection<BillProducts>? BillProducts { get; set; }
        public ICollection<CustomerOrdersProducts>? CustomerOrdersProducts { get; set; }
      


    }
}
