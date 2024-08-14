using Microsoft.EntityFrameworkCore;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly StorageDbContext _context;

        public CategoryRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task<SimpleCategoryDto> AddCategoryAsync(SimpleCategoryDto simpleCategoryDto)
        {
            var category = new Categories
            {
                Name = simpleCategoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new SimpleCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is not null) 
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Select(C => new CategoryDto
            {
                Id = C.Id,
                Name = C.Name,
            }).ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id, int pageNumber, int pageSize)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .ThenInclude(p => p.ProductDates)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return null; // or throw an exception
            }

            var totalProducts = category.Products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = category.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Unit = p.Unit,
                    Notes = p.Notes,
                    ProductDatesDto = p.ProductDates.Select(pd => new ProductDatesDto
                    {
                        AddDate = pd.AddDate,
                        ExpiryDate = pd.ExpiryDate
                    }).ToList(),
                    Quantity = p.Quantity,
                    Category = new SimpleCategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    },
                    //CurrentStock = p.CurrentStock,
                    TotalStock = p.TotalStock
                }).ToList();

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Products = products,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalProducts = totalProducts,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            };

            return categoryDto;
        }



        public async Task UpdateCategoryAsync(SimpleCategoryDto categoryDto)
        {
            // Retrieve the existing category from the database
            var category = await _context.Categories.FindAsync(categoryDto.Id);

            if (category == null)
            {
                
                throw new Exception("القسم غير متواجد");
            }

            // Update the properties of the existing category
            category.Name = categoryDto.Name;

            // Mark the category as modified
            _context.Categories.Update(category);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }


    }
}
