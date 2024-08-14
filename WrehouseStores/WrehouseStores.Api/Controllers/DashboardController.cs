using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.CustomerOrderDtos;
using WarehouseStores.Core.Dto.DashboardDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{

    public class DashboardController : BaseApiController
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("ProductsInStorage")]
        public async Task<ActionResult<IEnumerable<StorageProductsDto>>> GetProductsByStorage(int storageId)
        {
            var products = await _dashboardRepository.GetProductsByStorageAsync(storageId);

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("storage/{storageId}/nearing-expiry")]
        public async Task<ActionResult> GetProductsNearingExpiryByStorage(int storageId)
        {
            var productsNearingExpiry = await _dashboardRepository.GetProductsNearingExpiryAsync(storageId);

            if (!productsNearingExpiry.Any())
            {
                return NotFound();
            }

            var totalCount = productsNearingExpiry.First().TotalCount;

            return Ok(new
            {
                TotalCount = totalCount,
                Products = productsNearingExpiry
            });
        }

        [HttpGet("storage/{storageId}/running-low")]
        public async Task<ActionResult<IEnumerable<ProductRunningLowDto>>> GetProductsRunningLowByStorage(int storageId)
        {
            var productsRunningLow = await _dashboardRepository.GetProductsRunningLowAsync(storageId);

            if (productsRunningLow == null || !productsRunningLow.Any())
            {
                return NotFound();
            }

            return Ok(productsRunningLow);
        }

        [HttpGet("storage/{storageId}/top-selling-products")]
        public async Task<ActionResult> GetTopSellingProductsByStorage(int storageId)
        {
            var (totalSales, topSellingProducts) = await _dashboardRepository.GetTopSellingProductsAsync(storageId);

            if (topSellingProducts == null || !topSellingProducts.Any())
            {
                return NotFound();
            }

            return Ok(new
            {
                TotalSales = totalSales,
                Products = topSellingProducts
            });
        }

        [HttpGet("storage/{storageId}/value")]
        public async Task<ActionResult<StorageValueDto>> GetStorageValue(int storageId)
        {
            var storageValue = await _dashboardRepository.GetStorageValueAsync(storageId);

            if (storageValue == null)
            {
                return NotFound();
            }

            return Ok(storageValue);
        }

        [HttpGet("storage/{storageId}/pending-orders-count")]
        public async Task<ActionResult<int>> GetPendingOrdersCount(int storageId)
        {
            var pendingOrdersCount = await _dashboardRepository.GetPendingOrdersChangeAsync(storageId);

            return Ok(new
            {
                Count = pendingOrdersCount
            });
        }

        [HttpGet("check-expired-products")]
        public async Task<ActionResult<IEnumerable<MissingProductsDto>>> CheckExpiredProducts()
        {
            var expiredProducts = await _dashboardRepository.CheckAndAddExpiredProductsAsync();

            if (expiredProducts == null || !expiredProducts.Any())
            {
                return NotFound("جميع المنتجات التالفة تم اضافتها لا يوجد تحديث");
            }

            return Ok(expiredProducts);
        }

        [HttpGet("missing-products")]
        public async Task<ActionResult<IEnumerable<MissingProductsDto>>> GetAllMissingProducts()
        {
            var missingProducts = await _dashboardRepository.GetAllMissingProductsAsync();

            if (missingProducts == null || !missingProducts.Any())
            {
                return NotFound();
            }

            return Ok(missingProducts);
        }
        [HttpGet("missing-products-count")]
        public async Task<ActionResult<int>> GetCountOfMissingProducts(int storageId)
        {
            var count = await _dashboardRepository.GetCountOfMissingProductsAsync(storageId);
            return Ok(count);
        }
        [HttpGet("missing-products-statistics")]
        public async Task<ActionResult<MissingProductsStatisticsDto>> GetMissingProductsStatistics(int storageId)
        {
            var statistics = await _dashboardRepository.GetMissingProductsStatisticsAsync(storageId);
            return Ok(statistics);
        }

        [HttpPost("add-missing-product")]
        public async Task<ActionResult> AddMissingProduct([FromBody] MissingProductsDto missingProductDto)
        {
            await _dashboardRepository.AddMissingProductAsync(missingProductDto);
            return Ok("Product Added");
        }

        [HttpDelete("delete-missing-product/{id}")]
        public async Task<ActionResult> DeleteMissingProduct(int id)
        {
            await _dashboardRepository.DeleteMissingProductAsync(id);
            return Ok("Product Deleted");
        }

        [HttpPut("update-missing-product")]
        public async Task<ActionResult> UpdateMissingProduct([FromBody] MissingProductsDto missingProductDto)
        {
            await _dashboardRepository.UpdateMissingProductAsync(missingProductDto);
            return Ok("Product Updated");
        }

        [HttpGet("Track-CustomerOrders-Status")]
        public async Task<ActionResult<CustomerOrderWithPaginationDto>> TrackCustomerOrdersStatusAsync(int pageNumber = 1, int pageSize = 10)
        {
           var status = await _dashboardRepository.TrackCustomerOrdersStatusAsync(pageNumber, pageSize);
            return Ok(status);
        }

        [HttpGet("Track-ProductionOrders-Status")]
        public async Task<ActionResult<CustomerOrderWithPaginationDto>> TrackProductionOrdersStatusAsync(int pageNumber = 1, int pageSize = 10)
        {
            var status = await _dashboardRepository.TrackProductionOrdersStatusAsync(pageNumber,pageSize);
            return Ok(status);
        }

        [HttpGet("Search-MissingProducts")]
        public async Task<ActionResult<IEnumerable<MissingProductsDto>>> SearchMissingProductsAsync(string? productName)
        {
            var missingProducts = await _dashboardRepository.SearchMissingProductsAsync(productName);
            if (missingProducts == null)
                return NotFound("لايوجد منتج تالف بهذا الاسم");
            return Ok(missingProducts);
        }

        [HttpGet("search-order-status")]
        public async Task<ActionResult<CustomerOrderStatusSearchDto>> SearchOrderStatusAsync(int id)
        {
            var result = await _dashboardRepository.SearchOrderStatusAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
