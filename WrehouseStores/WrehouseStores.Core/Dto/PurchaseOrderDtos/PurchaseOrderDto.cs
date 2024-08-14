using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.StatusDtos;

namespace WarehouseStores.Core.Dto.PurchaseOrderDtos
{
    public class PurchaseOrderDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
       
        public string? Notes { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public DepartmentDto? DepartmentDto { get; set; }
        public ICollection<PurchaseOrderProductDto>? Products { get; set; }
        public PriorityDto? PriorityDto { get; set; }
        public StatusDto? StatusDto { get; set; }
    }
}
