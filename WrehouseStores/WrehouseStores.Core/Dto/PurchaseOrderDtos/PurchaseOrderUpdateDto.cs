using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Dto.PurchaseOrderDtos
{
    public class PurchaseOrderUpdateDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        // public DateOnly? AvailabilityDate { get; set; } 
        public string? Notes { get; set; } // Updated notes
        [EmailAddress]
        public string? Email { get; set; } // Updated email
        [Phone]
        public string? Phone { get; set; } // Updated phone
        public string? DepartmentName { get; set; } // Updated department name
        public ICollection<SimpleProductDto>? Products { get; set; } // Updated product list
        public string? PriorityName { get; set; } // Updated priority name
        public string? StatusName { get; set; } // Updated status name
    }
}
