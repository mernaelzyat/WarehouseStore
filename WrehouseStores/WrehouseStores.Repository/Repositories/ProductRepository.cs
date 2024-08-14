using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.CategoryDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StorageDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(StorageDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ProductDto> AddProductAsync(ProductAddDto productAddDto)
        {
            if (productAddDto == null)
            {
                throw new ArgumentNullException(nameof(productAddDto), "من فضلك ادخل البيانات كاملة وصحيحة");
            }
            
            var product = new Products
            {
                Name = productAddDto.Name,
                Unit = productAddDto.Unit,
                Notes = productAddDto.Notes,
                Quantity = productAddDto.Quantity ?? 0,
                Weight = productAddDto.Weight,
                Price = productAddDto.Price ?? 0,
                SalesTax = productAddDto.SalesTax ?? 0,
                Description = productAddDto.Description,
                TotalStock = productAddDto.TotalStock ?? 0,
                CategoryId = productAddDto.Category?.Id,
                ProductDates = productAddDto.ProductDatesDto?.Select(pd => new ProductDates
                {
                    AddDate = pd.AddDate,
                    ExpiryDate = pd.ExpiryDate
                }).ToList() ?? new List<ProductDates>()
            };

            try
            {

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new ArgumentException("حدث خطأ اثناء الحفظ");
            }

            return _mapper.Map<ProductDto>(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductDates)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is not null)
            {
                // delete related ProductDates
                if (product.ProductDates != null && product.ProductDates.Any())
                {
                    _context.ProductDates.RemoveRange(product.ProductDates);
                }

                // delete product
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<ProductWithPaginationDto> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var totalProducts = await _context.Products.CountAsync();

            var products = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Unit = p.Unit,
                    Quantity = p.Quantity,
                    Notes = p.Notes,
                    Price = p.Price,
                    SalesTax = p.SalesTax,
                    Description = p.Description,
                    ProductDatesDto = p.ProductDates.Select(pd => new ProductDatesDto
                    {
                        AddDate = pd.AddDate,
                        ExpiryDate = pd.ExpiryDate
                    }).ToList(),
                    Category = new SimpleCategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                })
                .ToListAsync();

            var response = new ProductWithPaginationDto
            {
                Products = products,
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Total = totalProducts,
                    HasNextPage = (pageNumber * pageSize) < totalProducts,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return response;
        }
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductDates) //ProductDates is included
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) throw new ArgumentException("المنتج غير متواجد");

            var productDetails = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Unit = product.Unit,
                Weight = product.Weight,
                Quantity = product.Quantity,
                Notes = product.Notes,
                SalesTax = product.SalesTax,
                Description = product.Description,
                ProductDatesDto = product.ProductDates?.Select(pd => new ProductDatesDto
                {
                    AddDate = pd.AddDate,
                    ExpiryDate = pd.ExpiryDate
                }).ToList(),
                Category = product.Category != null ? new SimpleCategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                } : null
            };

            return productDetails;
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string? productName = null,DateTime? expiryDate = null)
        {
            var query = _context.Products
                .Include(P => P.Category)
                .Include(P => P.ProductDates)
                .AsQueryable();

            // Search by ProductName or categoryName
            if (!string.IsNullOrEmpty(productName))
                query = query.Where(P => P.Name.Contains(productName)||P.Category.Name.Contains(productName));

            // Search by Date => Filter
            if (expiryDate.HasValue)
                query = query.Where(P => P.ProductDates.Any(pd => pd.ExpiryDate == expiryDate.Value));

            var products = await query.Select(P => new ProductDto
            {
                Id = P.Id,
                Name = P.Name,
                Unit = P.Unit,
                Quantity = P.Quantity,
                Price = P.Price,
                SalesTax = P.SalesTax,
                Description = P.Description,
                Notes = P.Notes,
                Category = P.Category != null ? new SimpleCategoryDto
                {
                    Id = P.Category.Id,
                    Name = P.Category.Name
                } : null,
                ProductDatesDto = P.ProductDates.Select(pd => new ProductDatesDto
                {
                    AddDate = pd.AddDate,
                    ExpiryDate = pd.ExpiryDate
                }).ToList()
            }).ToListAsync();

            return products;
        }



        public async Task UpdateProductAsync(ProductUpdateDto productUpdateDto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productUpdateDto.Id);

            if (product == null)
            {
                throw new ArgumentException($"المنتج {productUpdateDto.Id} غير متواجد.");
            }

            product.Name = productUpdateDto.Name;
            product.Unit = productUpdateDto.Unit;
            product.Notes = productUpdateDto.Notes;
            product.Quantity = productUpdateDto.Quantity;
            product.Weight = productUpdateDto.Weight;
            product.Price = productUpdateDto.Price ?? product.Price;
            product.SalesTax = productUpdateDto.SalesTax ?? product.SalesTax;
            product.Description = productUpdateDto.Description ?? product.Description;
           
            product.TotalStock = productUpdateDto.TotalStock ?? product.TotalStock;

            if (productUpdateDto.CategoryId.HasValue)
            {
                product.CategoryId = productUpdateDto.CategoryId;
            }

            if (productUpdateDto.ProductDatesDto != null)
            {
                
                product.ProductDates = productUpdateDto.ProductDatesDto.Select(pd => new ProductDates
                {
                    AddDate = pd.AddDate,
                    ExpiryDate = pd.ExpiryDate
                }).ToList();
            }

            await _context.SaveChangesAsync();
        }

    }
}
