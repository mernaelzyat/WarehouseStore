using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Departments
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<PurchaseOrders>? PurchaseOrders { get; set; }
        public ICollection<ReceivedOrders>? ReceivedOrders { get; set; }
        public ICollection<CustomerOrders>? CustomerOrders { get; set; }
        public ICollection<StorageDepartment>? StorageDepartments { get; set; }
    }
}
