using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Dto.BillDtos
{
    public class AddBillRequestDto
    {
        public int? OrderId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentName { get; set; }
        public string StatusName { get; set; }
        public string PriorityName { get; set; }
        public string CustomerName { get; set; }
        public List<ProductDto> Products { get; set; }
        public string EmployeeName { get; set; }
       
    }

}
