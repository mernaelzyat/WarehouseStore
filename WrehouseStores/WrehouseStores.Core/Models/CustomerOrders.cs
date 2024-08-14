using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class CustomerOrders
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }
        public Departments? Department { get; set; }
       // public string? ReasonOfOrder { get; set; }
       // public decimal? Weight { get; set; }
        public DateTime? Time { get; set; }
        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }
        public Status? Status { get; set; }
        public string? Notes { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        //public ICollection<Products>? Products { get; set; }
        public ICollection<ReadyOrders>? ReadyOrders { get; set; }

        public ICollection<CustomerOrdersProducts>? CustomerOrdersProducts { get; set; }
    }
}
