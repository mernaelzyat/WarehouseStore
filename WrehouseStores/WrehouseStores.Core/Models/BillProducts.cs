using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class BillProducts

    {
        
        [ForeignKey("BillId")]
        public int? BillId { get; set; }
        public Bill? Bill { get; set; }
        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        public Products? Product { get; set; }
        public int? Quantity { get; set; }

        public decimal Price { get; set; }
    }

}
