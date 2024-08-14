using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Dto.RecievedOrderDtos
{
    public class RecievedOrderWithStorageDto
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
        public string? DepartmentName { get; set; }
        public List<ProductWithStorageDto>? Products { get; set; }
    }


}
