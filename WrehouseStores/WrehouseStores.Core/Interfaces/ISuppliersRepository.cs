using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.SupplierDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface ISuppliersRepository
    {
        Task<SupplierWithPaginationDto> GatAllSuppliersAsync(int pageNumber, int pageSize);
        Task AddSupplierAsync(SupplierDto supplier);
        Task UpdateSupplierAsync(SupplierDto supplier);
        Task DeleteSupplierAsync(int id);
        Task<SupplierDto> GetSupplierByIdAsync(int id);
        Task<IEnumerable<SupplierDto>> SearchSuppliersAsync(string? name = null);
    }
}
