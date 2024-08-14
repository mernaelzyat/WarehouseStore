
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto.BillDtos;
using WarehouseStores.Core.Models;

namespace WarehouseStores.Core.Interfaces
{
    public interface IBillRepository
    {

        Task<BillDto> AddPurchaseBillAsync(AddSimpleBillRequestDto  requestDto);
        Task<BillDto> AddSalesBillAsync(AddSimpleBillRequestDto requestDto);
        Task UpdatePurchaseBillAsync(AddSimpleBillRequestDto billDto);
        Task UpdateSalesBillAsync(AddSimpleBillRequestDto billDto);
        Task<BillDto> GetBillByIdAsync(int id);
        Task<ProductionBillWithPaginationDto> GetSupplyFormForProductionAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ProductionBillDto>> SearchProductionBillsAsync(DateTime? date, int? month, int? year);
        Task<SalesSupplyFormWithPaginationDto> GetSalesSupplyFormAsync(int pageNumber, int pageSize);
        Task<IEnumerable<SalesSupplyFormDto>> SearchSalesSupplyFormAsync(DateTime? date, int? month, int? year);

    }
}
