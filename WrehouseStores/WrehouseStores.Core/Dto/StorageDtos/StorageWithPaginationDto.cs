using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseStores.Core.Dto.StorageDtos
{
    public class StorageWithPaginationDto
    {
        public PaginationDto Pagination { get; set; }
        public List<StorageDto> Storages { get; set; }
    }
}
