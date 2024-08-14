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
    public class ReadyOrdersDto
    {
        public int? Id { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public DepartmentDto DepartmentDto { get; set; }
        public CustomerOrdersDto? CustomerOrdersDto { get; set; }
        public StatusDto? StatusDto { get; set; }
        public ICollection<ProductDto> ProductDtos { get; set; }
    }
}
