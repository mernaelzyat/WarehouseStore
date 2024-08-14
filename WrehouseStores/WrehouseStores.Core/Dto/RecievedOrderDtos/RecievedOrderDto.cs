using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.BillDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.PurchaseOrderDtos;

namespace WarehouseStores.Core.Dto.RecievedOrderDtos
{
    public class RecievedOrderDto
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

        public PurchaseOrderDto? PurchaseOrderDto { get; set; }
        public List<ProductDto>? Products { get; set; }
        public DepartmentDto? Department { get; set; }
        public BillDto? Bill { get; set; }



    }
}
