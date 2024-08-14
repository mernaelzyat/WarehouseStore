using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class ProductDates
    {
        public int Id { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        public Products? Product { get; set; }
    }
}
