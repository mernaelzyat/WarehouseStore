using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.CustomerOrderDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface ICustomerOrdersRepository
    {
        Task<CustomerOrderWithPaginationDto> GetAllCustomerOrdersAsync(int pageNumber, int pageSize);
        Task<ReadyOrdersWithPaginationDto> GetAllReadyOrdersAsync(int pageNumber, int pageSize);
        Task<ReadyOrdersDto> AddReadyOrderAsync(AddReadyOrderDto addReadyOrderDto);

        Task<CustomerOrdersDto> AddCustomerOrderAsync(AddCustomerOrderRequestDto requestDto);
        Task<CustomerOrdersDto> GetCustomerOrderByIdAsync(int id);
        Task<ReadyOrdersDto> GetReadyOrderByIdAsync(int id);
        Task DeleteCustomerOrderAsync(int id);
        Task DeleteReadyOrderAsync(int id);



        Task UpdateCustomerOrderAsync(CustomerOrderUpdateDto customerOrderDto);
        Task<IEnumerable<CustomerOrdersDto>> SearchCustomerOrdersAsync(DateTime? availabilityDate = null, string? status = null);
        Task<IEnumerable<ReadyOrdersDto>> SearchReadyOrdersAsync(DateTime? date = null, string? status = null);

        Task UpdateReadyOrderAsync(ReadyOrderUpdateDto readyOrderDto);

    }
}
