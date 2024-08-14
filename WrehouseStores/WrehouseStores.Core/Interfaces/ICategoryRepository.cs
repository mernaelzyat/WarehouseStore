using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.CategoryDtos;
using WarehouseStores.Core.Models;

namespace WarehouseStores.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id, int pageNumber , int pageSize);
        Task<SimpleCategoryDto> AddCategoryAsync(SimpleCategoryDto simpleCategoryDto);
        Task UpdateCategoryAsync(SimpleCategoryDto category);
        Task DeleteCategoryAsync(int id);






    }
}
