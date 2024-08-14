using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.PurchaseOrderDtos;
using WarehouseStores.Core.Models;

namespace WarehouseStores.Core.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<PurchaseOrderWithPagination> GetAllPurchaseOrderAsync(int pageNumber, int pageSize);
        Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int id);
        Task AddPurchaseOrderAsync(PurchaseOrderAddDto purchaseOrderAddDto);
        Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(PurchaseOrderUpdateDto purchaseOrder);
        Task DeletePurchaseOrderAsync(int id);

        Task<IEnumerable<PurchaseOrderDto>> SearchPurchaseOrdersAsync(string status = null, DateTime? Date = null);


    }
}
