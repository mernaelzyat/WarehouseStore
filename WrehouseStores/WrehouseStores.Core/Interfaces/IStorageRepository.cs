
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.StorageDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface IStorageRepository
    {
        Task<StorageWithPaginationDto> GetAllStoragesWithPaginationAsync(int pageNumber, int pageSize);
        Task<List<StorageDto>> GetAllStoragesAsync();
        Task<StorageResponseDto> AddStorageAsync(StorageDto storageDto);
        Task DeleteStorageAsync( int id);
        Task UpdateStorageAsync(StorageDto storageDto);
        Task<IEnumerable<StorageDto>> SearchStoragesAsync(string? name = null );
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    }
}
