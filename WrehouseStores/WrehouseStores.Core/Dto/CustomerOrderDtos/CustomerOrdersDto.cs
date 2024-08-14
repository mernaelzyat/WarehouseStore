using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.StatusDtos;

namespace WarehouseStores.Core.Dto.CustomerOrderDtos
{
    public class CustomerOrdersDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime? AvailabilityDate { get; set; }


        public DateTime? Time { get; set; }
        public string? Notes { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public List<ProductDto>? Products { get; set; }
        public StatusDto? StatusDto { get; set; }
        public DepartmentDto? DepartmentDto { get; set; }


    }
}
