using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.BillDtos
{
    public class ProductionBillDto
    {
        public int? Id { get; set; }
        public string DepartmentName { get; set; }
        public string PriorityName { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; }
    }
}
