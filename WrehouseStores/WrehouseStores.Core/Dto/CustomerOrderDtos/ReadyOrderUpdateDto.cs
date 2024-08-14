using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Dto.CustomerOrderDtos
{
    public class ReadyOrderUpdateDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; set; }
        public DateTime Time { get; set; }
        public string StatusName { get; set; }
        // public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        // public string ReasonOfOrder { get; set; }
        public List<ProductUpdateDto>? Products { get; set; }
    }


}
