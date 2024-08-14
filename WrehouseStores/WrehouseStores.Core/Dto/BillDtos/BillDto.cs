using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.CustomerDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.RecievedOrderDtos;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Dto.SupplierDtos;

namespace WarehouseStores.Core.Dto.BillDtos
{
    public class BillDto
    {
        public int? Id { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public DepartmentDto? Department { get; set; }
        public PriorityDto? Priority { get; set; }
        public StatusDto? Status { get; set; }
        public SupplierDto? Supplier { get; set; }
        public CustomerDto? Customer { get; set; }
        public EmployeeDto? Employee { get; set; }
        public int OrderId { get; set; }
        public ICollection<ProductDto>? Products { get; set; }
        public ICollection<RecievedOrderDto> RecievedOrders { get; set; }
    }
}
