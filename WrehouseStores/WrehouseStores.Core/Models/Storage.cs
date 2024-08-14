using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Storage
    {
        public int? Id { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Supervisor { get; set; }
        public ICollection<Floor>? Floors { get; set; }
        public ICollection<Shelf>? Shelves { get; set; }

        public ICollection<Categories>? Categories { get; set; }
        public ICollection<ReceivedOrders>? ReceivedOrders { get; set; }
        public ICollection<StorageDepartment> StorageDepartments { get; set; }



    }
}
