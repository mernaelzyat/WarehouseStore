using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.BillDtos
{
    public class SalesSupplyFormDto
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; }
        public int? Quantity { get; set; }
    }
}
