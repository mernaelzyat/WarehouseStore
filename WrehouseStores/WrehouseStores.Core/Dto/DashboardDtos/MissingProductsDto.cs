using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.DashboardDtos
{
    public class MissingProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProductId { get; set; }

        public DateTime ExpiryDate { get; set; }
        public string ReasonOfMissing { get; set; }
    }
}
