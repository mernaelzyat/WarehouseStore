using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Models;

namespace WarehouseStores.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductWithPaginationDto> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> AddProductAsync(ProductAddDto productAddDto);
        Task UpdateProductAsync(ProductUpdateDto productUpdateDto);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string? productName = null, /*string? categoryName = null,*/ DateTime? date = null);
    }
}
