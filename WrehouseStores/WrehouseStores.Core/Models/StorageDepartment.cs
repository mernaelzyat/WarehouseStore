using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Models
{
    public class StorageDepartment
    {
        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }
        public Departments Department { get; set; }
        [ForeignKey("StorageId")]
        public int? StorageId { get; set; }
        public Storage Storage { get; set; }
    }
}
