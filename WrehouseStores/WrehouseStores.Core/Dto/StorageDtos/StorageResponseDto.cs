using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.StorageDtos
{
    public class StorageResponseDto
    {
        public int? Id { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Supervisor { get; set; }
        public List<DepartmentDto>? Departments { get; set; }
    }
}
