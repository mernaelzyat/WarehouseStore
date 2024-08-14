
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.RecievedOrderDtos;

namespace WarehouseStores.Core.Interfaces
{
    public interface IReceivedOrder
    {
        Task<RecievedOrderWithPaginationDto> GetAllRecievedOrdersAsync(int pageNumber, int pageSize);
        Task<RecievedOrderWithPaginationDto> GetAllStoredOrdersAsync(int pageNumber, int pageSize);
        Task DeleteStoredOrderAsync(int id);
        Task<IEnumerable<RecievedOrderDto>> SearchRecievedOrdersAsync(DateTime? date = null, int? OrderId = null);
        Task<IEnumerable<RecievedOrderDto>> SearchStoredOrdersAsync(DateTime? date = null, string? searcText = null);
        Task<RecievedOrderWithStorageDto> GetRecivedOrderByIdAsync(int id);
       // Task<RecievedOrderDto> GetStoredOrderByIdAsync(int id);
        

    }
}
