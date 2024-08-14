using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Bill
    {
        public int? Id { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? Date { get; set; }

        [ForeignKey("PriorityId")]
        public int? PriorityId { get; set; }
        public Priority? Priority { get; set; }
      
        [ForeignKey("SupplierId")]
        public int? SupplierId { get; set; }
        public Suppliers? Supplier { get; set; }
        public string? Notes { get; set; }
        public decimal? TotalPrice { get; set; }
        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }
        public Status? Status { get; set; }
        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }
        public Departments? Department { get; set; }
        public ICollection<BillProducts>? BillProducts { get; set; }
        public ICollection<ReceivedOrders>? ReceivedOrders { get; set; }
        [ForeignKey("EmployeeId")]
        public int? EmployeeId { get; set; }
        public Employees? Employee { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("CustomerId")]
        public int? CustomerId { get; set; }
        public Customers? Customers { get; set; }


    }
}
