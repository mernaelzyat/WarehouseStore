using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Categories
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Products>? Products { get; set; }
        public ICollection<Storage>? Storages { get; set; }


    }
}
