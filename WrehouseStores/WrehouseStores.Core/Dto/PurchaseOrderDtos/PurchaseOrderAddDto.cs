using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Dto.PurchaseOrderDtos
{
    public class PurchaseOrderAddDto
    {
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        // public DateOnly? AvailabilityDate { get; set; }
        public string? Notes { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? DepartmentName { get; set; } // Use name instead of ID

        public ICollection<SimpleProductDto>? Products { get; set; }
        public string? PriorityName { get; set; } // Use name instead of ID
        public string? StatusName { get; set; } // Use name instead of ID
    }

}
