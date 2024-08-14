using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class Stock
    {
        public int Id { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Products? Product { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal TotalStock { get; set; }
        public string? Status { get; set; }
    }
}
