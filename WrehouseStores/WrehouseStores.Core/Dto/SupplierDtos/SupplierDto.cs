using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.SupplierDtos
{
    public class SupplierDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }

    }
}
