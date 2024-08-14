using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Shelf
    {
        public int? Id { get; set; }
        public int? ShelfNumber { get; set; }
        public ICollection<Storage>? Storages { get; set; }
    }
}
