using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class ReadyOrders
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; set; }
        public DateTime Time { get; set; }

        
        public ICollection<CustomerOrders> CustomerOrders { get; set; }

        // Foreign keys and navigation properties
        public int? StatusId { get; set; }
        public Status Status { get; set; }

        public int? DepartmentId { get; set; }
        public Departments Department { get; set; }

        // Additional properties or collections as needed
        // public ICollection<Products> Products { get; set; }

        public ICollection<ReadyOrdersProducts> ReadyOrderProducts { get; set; }
    }
}
