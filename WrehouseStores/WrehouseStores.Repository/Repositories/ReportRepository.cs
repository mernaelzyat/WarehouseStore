using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.ReportDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly StorageDbContext _context;

        public ReportRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockReportDto>> GetStockReportsOverTimeAsync(string productName = null)
        {
            var query = _context.ProductDates
                .Include(pd => pd.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(pd => pd.Product.Name == productName);
            }

            var stockReports = await query
                .GroupBy(pd => new
                {
                    // Handling nullable DateOnly by using GetValueOrDefault() and defaulting to DateOnly.MinValue if null
                    Month = pd.AddDate.HasValue ? pd.AddDate.Value.Month : 1,
                    Year = pd.AddDate.HasValue ? pd.AddDate.Value.Year : DateTime.Now.Year,
                    ProductName = pd.Product.Name
                })
                .Select(g => new StockReportDto
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    ProductName = g.Key.ProductName,
                    Quantity = g.Sum(pd => pd.Product.Quantity),
                    TotalStock = g.Sum(pd => pd.Product.TotalStock)
                })
                .ToListAsync();

            return stockReports;
        }


        public async Task<IEnumerable<StockStatusDto>> GetCurrentStockStatusAsync()
        {
            var currentStockStatus = await _context.Products
                 .GroupBy(p => new { p.Name, p.Unit })

                .Select(g => new StockStatusDto
                {
                    ProductName = g.Key.Name,
                    Quantity = g.Sum(p => p.Quantity),
                    Unit = g.Key.Unit,
                    Status = g.Sum(p => p.Quantity) < 100 ? "منخفضة" : "طبيعية" // Example status based on total quantity
                })
                .ToListAsync();

            return currentStockStatus;
        }

        public async Task<IEnumerable<SearchReportDto>> SearchReportAsync(string productName)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.Name.Contains(productName));
            }

            var searchResults = await query
                .Select(p => new SearchReportDto
                {
                    ProductName = p.Name,
                    Quantity = p.Quantity,
                    TotalStock = p.TotalStock
                })
                .ToListAsync();

            return searchResults;
        }
    }
}
