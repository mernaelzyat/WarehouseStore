﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.PurchaseOrderDtos
{
    public class PurchaseOrderProductDto
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }

        public decimal? Weight { get; set; }
    }
}
