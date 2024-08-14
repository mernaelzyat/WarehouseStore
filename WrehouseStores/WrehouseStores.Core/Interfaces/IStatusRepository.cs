using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.StatusDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface IStatusRepository
    {
        Task<IEnumerable<StatusDto>> GetAllStatusAsync();
        Task<IEnumerable<PriorityDto>> GetAllPriorityAsync();
    }
}
