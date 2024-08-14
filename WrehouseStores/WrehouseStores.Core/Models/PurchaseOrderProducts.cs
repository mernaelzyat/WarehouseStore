using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class PurchaseOrderProducts
    {
        [Key]
        public int Id { get; set; }

        public int? PurchaseOrderId { get; set; }
        public PurchaseOrders? PurchaseOrder { get; set; }

        public int? ProductId { get; set; }
        public Products? Product { get; set; }

        public int? Quantity { get; set; } // Quantity specific to this purchase order

        public decimal? Weight { get; set; }
    }

}
