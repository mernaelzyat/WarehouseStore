using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.CustomerDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task <CustomerWithPaginationDto> GatAllCustomersAsync(int pageNumber, int pageSize);
        Task AddCustomerAsync(CustomerDto customer);
        Task UpdateCustomerAsync(CustomerDto customer);
        Task DeleteCustomerAsync(int id);
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string? name = null);

    }
}
