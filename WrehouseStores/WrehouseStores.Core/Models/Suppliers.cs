using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Suppliers
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public ICollection<Bill>? Bills { get; set; }
    }
}
