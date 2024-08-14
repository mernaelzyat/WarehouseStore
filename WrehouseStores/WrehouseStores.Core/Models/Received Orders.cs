using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class ReceivedOrders
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }
        public Departments? Department { get; set; }
        [ForeignKey("StorageId")]
        public int? StorageId { get; set; }
        public Storage? Storage { get; set; }
        //public decimal Weight { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public int? PurchaseOrderId { get; set; }
        public PurchaseOrders? PurchaseOrder { get; set; }
        [ForeignKey("BillId")]
        public int BillId { get; set; }
        public Bill? Bill { get; set; }
        public ICollection<Products>? Products { get; set; }

    }
}
