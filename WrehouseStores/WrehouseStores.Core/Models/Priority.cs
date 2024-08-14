using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Priority
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Bill>? Bills { get; set; }
        public ICollection<PurchaseOrders>? PurchaseOrders { get; set; }
    }
}
