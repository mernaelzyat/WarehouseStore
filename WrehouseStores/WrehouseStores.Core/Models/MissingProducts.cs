using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class MissingProducts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProductId { get; set; } // Remove the ForeignKey attribute
        public DateTime ExpiryDate { get; set; }
        public string ReasonOfMissing { get; set; }
    }

}
