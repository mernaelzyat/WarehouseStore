using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.SupplierDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{

    public class SupplierController : BaseApiController
    {
        private readonly ISuppliersRepository _suppliersRepository;

        public SupplierController(ISuppliersRepository suppliersRepository)
        {
            _suppliersRepository = suppliersRepository;
        }

        [HttpGet("AllSuppliers")]
        public async Task<SupplierWithPaginationDto> GetAllSuppliersAsync(int pageNumber = 1, int pageSize = 10 )
        {
            var suppliers = await _suppliersRepository.GatAllSuppliersAsync( pageNumber,  pageSize);
            return suppliers;
        }

        [HttpDelete("DeleteSupplier")]
        public async Task<ActionResult> DeleteSupplierAsync(int id )
        {
            await _suppliersRepository.DeleteSupplierAsync(id);
            return Ok("Supplier deleted");
        }

        [HttpPost("AddSupplier")]
        public async Task<ActionResult> AddSupplierAsync(SupplierDto supplierDto)
        {
            await _suppliersRepository.AddSupplierAsync(supplierDto);
            return Ok("Supplier Added");
        }

        [HttpPut("UpdateSupplier")]
        public async Task<ActionResult> UpdateSupplierAsync(int id , SupplierDto supplierDto)
        {
            if (id != supplierDto.Id) return BadRequest();

            await _suppliersRepository.UpdateSupplierAsync(supplierDto);
            return Ok(supplierDto);
        }

        [HttpGet("SupplierById")]
        public async Task<ActionResult> GetSupplierById(int id)
        {
            var supplier = await _suppliersRepository.GetSupplierByIdAsync(id);
            return Ok(supplier);
        }

        [HttpPost("SearchSupplier")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> SearchSuppliersAsync(string? name = null)
        {
            var suppliers = await _suppliersRepository.SearchSuppliersAsync(name);
            if(!suppliers.Any()) return BadRequest();

            return Ok(suppliers);
        }

    }
}
