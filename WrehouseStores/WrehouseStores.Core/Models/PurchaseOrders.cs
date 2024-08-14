using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class PurchaseOrders
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }
        public Departments? Department { get; set; }
        public DateTime? Date { get; set; }
       // public DateOnly? AvailabilityDate { get; set; }
        public string? Notes { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [ForeignKey("PriorityId")]
        public int? PriorityId { get; set; }
        public Priority? Priority { get; set; }
        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }
        public Status? Status { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public ReceivedOrders? ReceivedOrder { get; set; }
        public ICollection<PurchaseOrderProducts>? PurchaseOrderProducts { get; set; }


    }
}
