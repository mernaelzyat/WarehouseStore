using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.CustomerOrderDtos;
using WarehouseStores.Core.Dto.DashboardDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly StorageDbContext _context;

        public DashboardRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task AddMissingProductAsync(MissingProductsDto missingProductDto)
        {
            // Retrieve the product from the Products table
            var product = await _context.Products
                .Where(p => p.Id == missingProductDto.ProductId && p.Name == missingProductDto.Name)
                .Include(p => p.ProductDates) // Include related data if needed
                .FirstOrDefaultAsync();

            // Check if the product exists
            if (product != null)
            {
                // Create a new MissingProducts entity
                var missingProduct = new MissingProducts
                {
                    ProductId = product.Id,
                    Name = missingProductDto.Name,
                    ExpiryDate = missingProductDto.ExpiryDate,
                    ReasonOfMissing = missingProductDto.ReasonOfMissing
                };

                // Add the missing product to the MissingProducts table
                _context.MissingProducts.Add(missingProduct);

                // Manually handle related records before deleting the product
                _context.ProductDates.RemoveRange(product.ProductDates);

                // Remove the product from the Products table
                _context.Products.Remove(product);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the product does not exist in the Products table
                throw new InvalidOperationException("The product does not exist in the Products table.");
            }
        }
        public async Task<IEnumerable<MissingProductsDto>> CheckAndAddExpiredProductsAsync()
        {
            var currentDate = DateTime.Now;
            var expiredProducts = await _context.Products
                .Include(p => p.ProductDates)
                .Where(p => p.ProductDates.Any(pd => pd.ExpiryDate.HasValue && pd.ExpiryDate.Value <= currentDate))
                .ToListAsync();

            var missingProductsDtos = new List<MissingProductsDto>();

            foreach (var product in expiredProducts)
            {
                var expiryDate = product.ProductDates.FirstOrDefault(pd => pd.ExpiryDate.HasValue && pd.ExpiryDate.Value <= currentDate).ExpiryDate.Value;

                var existingProduct = await _context.MissingProducts
                    .FirstOrDefaultAsync(mp => mp.ProductId == product.Id && mp.ExpiryDate == expiryDate);

                if (existingProduct == null)
                {
                    var missingProduct = new MissingProducts
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        ExpiryDate = expiryDate,
                        ReasonOfMissing = "منتهي الصلاحية"
                    };

                    _context.MissingProducts.Add(missingProduct);
                    missingProductsDtos.Add(new MissingProductsDto
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        ExpiryDate = expiryDate,
                        ReasonOfMissing = "منتهي الصلاحية"
                    });
                }
            }

            await _context.SaveChangesAsync();
            return missingProductsDtos;
        }
        public async Task DeleteMissingProductAsync(int id)
        {
            var missingProduct = await _context.MissingProducts.FindAsync(id);
            if (missingProduct != null)
            {
                _context.MissingProducts.Remove(missingProduct);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<MissingProductsDto>> GetAllMissingProductsAsync()
        {
            var missingProducts = await _context.MissingProducts
                .Select(mp => new MissingProductsDto
                {
                    Id = mp.Id,
                    ProductId = mp.ProductId,
                    Name = mp.Name,
                    ExpiryDate = mp.ExpiryDate,
                    ReasonOfMissing = mp.ReasonOfMissing
                })
                .ToListAsync();

            return missingProducts;
        }
        public async Task<int> GetCountOfMissingProductsAsync(int storageId)
        {
            var count = await _context.MissingProducts
                .Where(mp => mp.ProductId.HasValue && _context.Products
                    .Where(p => p.Category.Storages.Any(s => s.Id == storageId))
                    .Select(p => p.Id)
                    .Contains(mp.ProductId.Value))
                .CountAsync();

            return count;
        }
        public async Task<MissingProductsStatisticsDto> GetMissingProductsStatisticsAsync(int storageId)
        {
            var currentMonth = DateTime.Now.Month;
            var previousMonth = DateTime.Now.AddMonths(-1).Month;

            // Count of missing products for the current month
            var currentMonthCount = await _context.MissingProducts
                .Where(mp => mp.ProductId.HasValue && _context.Products
                    .Where(p => p.Category.Storages.Any(s => s.Id == storageId))
                    .Select(p => p.Id)
                    .Contains(mp.ProductId.Value)
                    && mp.ExpiryDate.Month == currentMonth)
                .CountAsync();

            // Count of missing products for the previous month
            var previousMonthCount = await _context.MissingProducts
                .Where(mp => mp.ProductId.HasValue && _context.Products
                    .Where(p => p.Category.Storages.Any(s => s.Id == storageId))
                    .Select(p => p.Id)
                    .Contains(mp.ProductId.Value)
                    && mp.ExpiryDate.Month == previousMonth)
                .CountAsync();

            // Calculate the percentage change
            var percentageChange = previousMonthCount > 0
                ? ((double)(currentMonthCount - previousMonthCount) / previousMonthCount) * 100
                : 100;

            return new MissingProductsStatisticsDto
            {
                CurrentMonthCount = currentMonthCount,
                PreviousMonthCount = previousMonthCount,
                PercentageChange = percentageChange
            };
        }

        public async Task<PendingOrdersChangeDto> GetPendingOrdersChangeAsync(int storageId)
        {
            var storage = await _context.Storage
                .Include(s => s.Categories)
                .ThenInclude(c => c.Products)
                .ThenInclude(p => p.CustomerOrdersProducts)
                .ThenInclude(cop => cop.CustomerOrder)
                .FirstOrDefaultAsync(s => s.Id == storageId);

            if (storage == null)
            {
                return null;
            }

            var currentMonth = DateTime.Now.Month;
            var lastMonth = DateTime.Now.AddMonths(-1).Month;

            var currentMonthPendingOrders = storage.Categories
                .SelectMany(c => c.Products)
                .SelectMany(p => p.CustomerOrdersProducts)
                .Count(cop => cop.CustomerOrder.StatusId == 1 && cop.CustomerOrder.AvailabilityDate.HasValue && cop.CustomerOrder.AvailabilityDate.Value.Month == currentMonth);

            var lastMonthPendingOrders = storage.Categories
                .SelectMany(c => c.Products)
                .SelectMany(p => p.CustomerOrdersProducts)
                .Count(cop => cop.CustomerOrder.StatusId == 1 && cop.CustomerOrder.AvailabilityDate.HasValue && cop.CustomerOrder.AvailabilityDate.Value.Month == lastMonth);

            var percentageChange = lastMonthPendingOrders == 0 ? 0 : ((currentMonthPendingOrders - lastMonthPendingOrders) / (double)lastMonthPendingOrders) * 100;

            return new PendingOrdersChangeDto
            {
                CurrentMonthCount = currentMonthPendingOrders,
                LastMonthCount = lastMonthPendingOrders,
                PercentageChange = percentageChange
            };
        }
        public async Task<IEnumerable<StorageProductsDto>> GetProductsByStorageAsync(int storageId)
        {
            var products = await _context.Storage
             .Where(s => s.Id == storageId)
             .SelectMany(s => s.Categories)
             .SelectMany(c => c.Products)
             
             .Select(p => new StorageProductsDto
             {
                 Id = p.Id,
                 Name = p.Name,
                 Unit = p.Unit,
                
                 Quantity = p.Quantity,
                 TotalStock = p.TotalStock,
                 StorageName = _context.Storage.FirstOrDefault(s => s.Id == storageId).Name
             })
             .ToListAsync();

            return products;
        }
        public async Task<IEnumerable<ProductsNearToExpireDto>> GetProductsNearingExpiryAsync(int storageId)
        {
            var storage = await _context.Storage
                .Include(s => s.Categories)
                .ThenInclude(c => c.Products)
                .ThenInclude(p => p.ProductDates)
                .FirstOrDefaultAsync(s => s.Id == storageId);


            //if (storage == null)

            var currentDate = DateTime.Now;
            var threeMonthsLater = currentDate.AddMonths(3);
            
            var productsNearingExpiryQuery = storage.Categories
                .SelectMany(c => c.Products)
                .SelectMany(p => p.ProductDates, (product, productDate) => new { product, productDate })
                .Where(pd => pd.productDate.ExpiryDate.HasValue && pd.productDate.ExpiryDate.Value <= threeMonthsLater)

                 .Select(p => new ProductsNearToExpireDto
                 {
                     Id = p.product.Id,
                     Name = p.product.Name,
                     ExpiryDate = p.productDate.ExpiryDate,
                     TotalCount = 1


                 })
            .ToList();


            var productsNearingExpiry = productsNearingExpiryQuery.ToList();
            var totalCount = productsNearingExpiry.Count;

            productsNearingExpiry.ForEach(p => p.TotalCount = totalCount);

            return productsNearingExpiry;


        }
        public async Task<IEnumerable<ProductRunningLowDto>> GetProductsRunningLowAsync(int storageId)
        {
            var storage = await _context.Storage
                .Include(s => s.Categories)
                .ThenInclude(c => c.Products)
                .FirstOrDefaultAsync(s => s.Id == storageId);

            if (storage == null)
            {
                return new List<ProductRunningLowDto>();
            }

            var products = storage.Categories
                 .SelectMany(c => c.Products)
                 .GroupBy(p => p.Name)
                 .Select(g => new ProductRunningLowDto
                 {
                     Name = g.Key,
                     TotalQuantity = g.Sum(p => p.Quantity ?? 0),
                     TotalStock = g.Sum(p => p.TotalStock ?? 0),
                     Percentage = (double)g.Sum(p => p.Quantity ?? 0) / g.Sum(p => p.TotalStock ?? 0) * 100
                    

                 })
                  .Where(p => p.Percentage <= 20)
                  .ToList();

            return products;

        }
        public async Task<StorageValueDto> GetStorageValueAsync(int storageId)
        {
            var storage = await _context.Storage
                .Include(s => s.Categories)
                .ThenInclude(c => c.Products)
                .FirstOrDefaultAsync(s => s.Id == storageId);

            if (storage == null || storage.Categories == null || !storage.Categories.Any())
            {
                return null;
            }

            var totalValue = storage.Categories
                .Where(c => c.Products != null)
                .SelectMany(c => c.Products)
                .Where(p => p.Quantity.HasValue)
                .Sum(p => (p.Quantity ?? 0) * p.Price);

            // Calculating the change for the past month
            var previousMonthTotalValue = storage.Categories
                .Where(c => c.Products != null)
                .SelectMany(c => c.Products)
                .Where(p => p.ProductDates != null && p.ProductDates.Any(d => d.AddDate.HasValue && d.AddDate.Value.Month == DateTime.Now.AddMonths(-1).Month)
                            && p.Quantity.HasValue)
                .Sum(p => (p.Quantity ?? 0) * p.Price);

            var percentageChange = previousMonthTotalValue == 0 ? 0 : (totalValue - previousMonthTotalValue) / previousMonthTotalValue * 100;

            return new StorageValueDto
            {
                TotalValue = totalValue,
                PercentageChange = percentageChange
            };
        }
        public async Task<(decimal TotalSales, IEnumerable<TopSellingProductDto> Products)> GetTopSellingProductsAsync(int storageId)
        {
            var storage = await _context.Storage
                .Include(s => s.Categories)
                .ThenInclude(c => c.Products)
                .ThenInclude(p => p.CustomerOrdersProducts)
                .ThenInclude(cop => cop.CustomerOrder)
                .FirstOrDefaultAsync(s => s.Id == storageId);

            if (storage == null)
            {
                return (0, new List<TopSellingProductDto>());
            }

            var topSellingProducts = storage.Categories
                .SelectMany(c => c.Products)
                .SelectMany(p => p.CustomerOrdersProducts)
                .GroupBy(p => p.Product.Name)
                .Select(g => new TopSellingProductDto
                {
                    Name = g.Key,
                    QuantitySold = g.Sum(p => p.Quantity ?? 0),
                    TotalSales = g.Sum(p => p.Quantity ?? 0 * p.Product.Price)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(5)  // top 5 Selling Products 
                .ToList();

            var totalSales = topSellingProducts.Sum(p => p.TotalSales);

            return (totalSales, topSellingProducts);
        }
        public async Task UpdateMissingProductAsync(MissingProductsDto missingProductDto)
        {
            // Retrieve the existing missing product entry
            var missingProduct = await _context.MissingProducts.FindAsync(missingProductDto.Id);

            if (missingProduct != null)
            {
                // Check if the referenced product exists
                var productExists = await _context.Products.AnyAsync(p => p.Id == missingProductDto.ProductId);

                if (productExists)
                {
                    // Update the missing product details
                    missingProduct.Name = missingProductDto.Name;
                    missingProduct.ProductId = missingProductDto.ProductId;
                    missingProduct.ExpiryDate = missingProductDto.ExpiryDate;
                    missingProduct.ReasonOfMissing = missingProductDto.ReasonOfMissing;

                    // Mark the entity as modified
                    _context.MissingProducts.Update(missingProduct);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Handle the case where the referenced product does not exist
                    throw new InvalidOperationException("The referenced product does not exist.");
                }
            }
            else
            {
                // Handle the case where the missing product entry was not found
                throw new InvalidOperationException("The missing product entry was not found.");
            }
        }
        public async Task<CustomerOrderWithPaginationDto> TrackCustomerOrdersStatusAsync(int pageNumber, int pageSize)
        {
            // Count total records where DepartmentId = 2 (Sales)
            var totalRecords = await _context.CustomerOrders
                .Where(o => o.DepartmentId == 2) // Filter for Sales department
                .CountAsync();

            // Fetch paginated and filtered customer orders
            var customerOrders = await _context.CustomerOrders
                .Where(o => o.DepartmentId == 2) // Filter for Sales department
                .OrderBy(o => o.Time)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new CustomerOrdersDto
                {
                    Id = o.Id,
                    Products = o.CustomerOrdersProducts.Select(p => new ProductDto
                    {
                        Id = p.Product.Id,
                        Name = p.Product.Name,
                        Unit = p.Product.Unit,
                        Quantity = p.Quantity,
                        Weight = p.Product.Weight,
                        Price = p.Product.Price
                    }).ToList(),
                    Time = o.Time,
                    StatusDto = new StatusDto
                    {
                        Id = o.Status.Id, // Assuming o.Status has Id
                        Name = o.Status.Name // Assuming o.Status has Name
                    }
                })
                .ToListAsync();

            // Create pagination DTO
            var pagination = new PaginationDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = totalRecords,
                HasNextPage = pageNumber * pageSize < totalRecords,
                HasPreviousPage = pageNumber > 1
            };

            // Return the paginated customer orders with pagination info
            return new CustomerOrderWithPaginationDto
            {
                CustomerOrders = customerOrders,
                Pagination = pagination
            };
        }
        public async Task<CustomerOrderWithPaginationDto> TrackProductionOrdersStatusAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.CustomerOrders
                .Where(o => o.DepartmentId == 1) // Filter for production orders
                .CountAsync();

            var customerOrders = await _context.CustomerOrders
                .Where(o => o.DepartmentId == 1) // Filter for production orders
                .OrderBy(o => o.Time)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new CustomerOrdersDto
                {
                    Id = o.Id,
                    Products = o.CustomerOrdersProducts.Select(p => new ProductDto
                    {
                        Id = p.Product.Id,
                        Name = p.Product.Name,
                        Unit = p.Product.Unit,
                        Quantity = p.Quantity,
                        Weight = p.Product.Weight,
                        Price = p.Product.Price
                    }).ToList(),
                    Time = o.Time,
                    StatusDto = new StatusDto
                    {
                        Id = o.Status.Id,
                        Name = o.Status.Name
                    }
                })
                .ToListAsync();

            var pagination = new PaginationDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = totalRecords,
                HasNextPage = pageNumber * pageSize < totalRecords,
                HasPreviousPage = pageNumber > 1
            };

            return new CustomerOrderWithPaginationDto
            {
                CustomerOrders = customerOrders,
                Pagination = pagination
            };
        }
        public async Task<IEnumerable<MissingProductsDto>> SearchMissingProductsAsync(string? productName = null)
        {
            var query = _context.MissingProducts.AsQueryable();

            if(!string.IsNullOrEmpty(productName))
                query = query.Where(m => m.Name.Contains(productName));

            var products = await query.Select(m => new MissingProductsDto
            {
                Id = m.Id,
                ProductId = m.ProductId,
                Name = m.Name,
                ExpiryDate = m.ExpiryDate,
                ReasonOfMissing = m.ReasonOfMissing
            }).ToListAsync();

            Console.WriteLine($"Query: {query.ToQueryString()}"); // Add this line to log the query
            Console.WriteLine($"Products Count: {products.Count()}"); // Add this line to log the count of products returned

            return products;

        }

        public async Task<CustomerOrderStatusSearchDto> SearchOrderStatusAsync(int id )
        {
            var order = await _context.CustomerOrders
          .Include(o => o.CustomerOrdersProducts)
              .ThenInclude(p => p.Product)
          .Include(o => o.Status)
          .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return null;
            }

            var result = new CustomerOrderStatusSearchDto
            {
                Id = order.Id,
                Products = order.CustomerOrdersProducts.Select(p => new SimpleProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    
                   
                }).ToList(),
                Time = order.Time,
                StatusDto = new StatusDto
                {
                    Id = order.Id,
                    Name = order.Status.Name,
                }
            };

            return result;
        }
    }
}


