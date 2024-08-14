using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("AllProducts")]
        public async Task<ProductWithPaginationDto> GetAllProducts(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productRepository.GetAllProductsAsync( pageNumber,  pageSize);
            return products;
        }

        [HttpGet("ProductById")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductAddDto productDto)
        {
            await _productRepository.AddProductAsync(productDto);
            
            return Ok("Product Added Successfully");
        }

        [HttpPut("UpdateProduct{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
        {
            if (id != productUpdateDto.Id) return BadRequest(ModelState);

            await _productRepository.UpdateProductAsync(productUpdateDto);
            return Ok(productUpdateDto);
        }

        [HttpDelete("DeleteProduct{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteProductAsync(id);
            return Ok("Product Deleted");
        }

        [HttpPost("SearchProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProductsAsync([FromBody] ProductsSearchRequestDto request)
        {
            var products = await _productRepository.SearchProductsAsync(request.ProductName, /*request.CategoryName,*/ request.ExpiryDate);
            if (!products.Any())
                return NotFound("No products found matching the criteria.");

            return Ok(products);
        }

    }
}
