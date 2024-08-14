using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.BillDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Repository.Repositories;

namespace WarehouseStores.Api.Controllers
{

    public class BillController : BaseApiController
    {
        private readonly IBillRepository _billRepository;

        public BillController(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        [HttpPost("AddPurchaseBill")]
        public async Task<ActionResult> AddPurchaseBill([FromBody] AddSimpleBillRequestDto billRequestDto)
        {
            if (billRequestDto == null)
            {
                return BadRequest(new { message = "BillAddDto is null" });
            }

            if (billRequestDto.Products == null || !billRequestDto.Products.Any())
            {
                return BadRequest(new { message = "At least one product is required." });
            }

            try
            {
                var addedBill = await _billRepository.AddPurchaseBillAsync(billRequestDto);
                return Ok("Bill Added Successefully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("AddSelesBill")]
        public async Task<ActionResult> AddSalesBill([FromBody] AddSimpleBillRequestDto billRequestDto)
        {
            if (billRequestDto == null)
            {
                return BadRequest(new { message = "BillAddDto is null" });
            }

            if (billRequestDto.Products == null || !billRequestDto.Products.Any())
            {
                return BadRequest(new { message = "At least one product is required." });
            }

            try
            {
                var addedBill = await _billRepository.AddSalesBillAsync(billRequestDto);
                return Ok("Bill Added Successefully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("BillById")]
        public async Task<ActionResult> GetBillById(int id)
        {
            var bill = await _billRepository.GetBillByIdAsync(id);
            if (bill == null) return NotFound();
            return Ok(bill);
        }
       

        [HttpPut("UpdatePurchaseBill")]
        public async Task<ActionResult> UpdatePurchaseBillAsync(int id,AddSimpleBillRequestDto billDto)
        {
            if (id != billDto.Id) return BadRequest(ModelState);

            await _billRepository.UpdatePurchaseBillAsync(billDto);
            return Ok(billDto);
        }

        [HttpPut("UpdateSalesBill")]
        public async Task<ActionResult> UpdateSalesBillAsync(int id, AddSimpleBillRequestDto billDto)
        {
            if (id != billDto.Id) return BadRequest(ModelState);

            await _billRepository.UpdateSalesBillAsync(billDto);
            return Ok(billDto);
        }

        [HttpGet("GetSupplyFormForProduction")]
        public async Task<ActionResult<IEnumerable<ProductionBillWithPaginationDto>>> GetSupplyFormForProduction(int pageNumber = 1, int pageSize = 10)
        {
            var productionBills = await _billRepository.GetSupplyFormForProductionAsync(pageNumber, pageSize);
            return Ok(productionBills);
        }

        [HttpGet("SearchProductionBills")]
        public async Task<ActionResult<IEnumerable<ProductionBillDto>>> SearchProductionBills([FromQuery] DateTime? date, [FromQuery] int? month, [FromQuery] int? year)
        {
            var productionBills = await _billRepository.SearchProductionBillsAsync(date, month, year);
            return Ok(productionBills);
        }
        [HttpGet("GetSalesSupplyForm")]
        public async Task<ActionResult<IEnumerable<SalesSupplyFormWithPaginationDto>>> GetSalesSupplyForm(int pageNumber = 1, int pageSize = 10)
        {
            var salesSupplyForms = await _billRepository.GetSalesSupplyFormAsync(pageNumber, pageSize);
            return Ok(salesSupplyForms);
        }

        [HttpGet("SearchSalesSupplyForm")]
        public async Task<ActionResult<IEnumerable<SalesSupplyFormDto>>> SearchSalesSupplyForm([FromQuery] DateTime? date, [FromQuery] int? month, [FromQuery] int? year)
        {
            var salesSupplyForms = await _billRepository.SearchSalesSupplyFormAsync(date, month, year);
            return Ok(salesSupplyForms);
        }


    }
}

