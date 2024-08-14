using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.CustomerOrderDtos;
using WarehouseStores.Core.Dto.DashboardDtos;
using WarehouseStores.Core.Dto.ProductDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<StorageProductsDto>> GetProductsByStorageAsync(int storageId);
        Task<IEnumerable<ProductsNearToExpireDto>> GetProductsNearingExpiryAsync(int storageId);
        Task<IEnumerable<ProductRunningLowDto>> GetProductsRunningLowAsync(int storageId);
        Task<(decimal TotalSales, IEnumerable<TopSellingProductDto> Products)> GetTopSellingProductsAsync(int storageId);
        Task<StorageValueDto> GetStorageValueAsync(int storageId);
        Task<PendingOrdersChangeDto> GetPendingOrdersChangeAsync(int storageId);
        Task<IEnumerable<MissingProductsDto>> CheckAndAddExpiredProductsAsync();
        Task<IEnumerable<MissingProductsDto>> GetAllMissingProductsAsync();

        Task<int> GetCountOfMissingProductsAsync(int storageId);
        Task<MissingProductsStatisticsDto> GetMissingProductsStatisticsAsync(int storageId);
        Task AddMissingProductAsync(MissingProductsDto missingProductDto);

        Task DeleteMissingProductAsync(int id);
        Task UpdateMissingProductAsync(MissingProductsDto missingProductDto);

        Task<CustomerOrderWithPaginationDto> TrackCustomerOrdersStatusAsync(int pageNumber, int pageSize);
        Task<CustomerOrderWithPaginationDto> TrackProductionOrdersStatusAsync(int pageNumber, int pageSize);
        Task<IEnumerable<MissingProductsDto>> SearchMissingProductsAsync(string? productName = null);
        Task<CustomerOrderStatusSearchDto> SearchOrderStatusAsync(int id);

    }
}
