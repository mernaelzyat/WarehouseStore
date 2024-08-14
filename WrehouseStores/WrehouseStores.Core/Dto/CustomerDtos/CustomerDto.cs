using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.CustomerDtos
{
    public class CustomerDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [MaxLength(50)]
        public string? CommercialRegister { get; set; }
        [Phone]
        public string? Phone { get; set; }
    }
}
