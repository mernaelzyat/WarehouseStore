using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class ReadyOrdersProducts
    {
        public int Id { get; set; }
        public int ReadyOrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
        public decimal? Weight { get; set; }

        public ReadyOrders ReadyOrder { get; set; }
        public Products Product { get; set; }
    }
}
