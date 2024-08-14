using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseStores.Core.Dto.PurchaseOrderDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;
using WarehouseStores.Repository.Repositories;

namespace WarehouseStores.Api.Controllers
{

    public class PurchaseOrderController : BaseApiController
    {
        private readonly IPurchaseRepository _purchaseOrderRepository;
        private readonly IMapper _mapper;
        private readonly StorageDbContext _context;

        public PurchaseOrderController(IPurchaseRepository purchaseOrderRepository, IMapper mapper, StorageDbContext context)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("AllPurchaseOrders")]
        public async Task<PurchaseOrderWithPagination> GetAllPurchaseOrdersAsync(int pageNumber = 1 , int pageSize = 10)
        {
            var orders = await _purchaseOrderRepository.GetAllPurchaseOrderAsync(pageNumber , pageSize);
            return orders;
        }

        [HttpGet("PurchaseOrderById")]
        public async Task<ActionResult> GetPurchaseOrderByIdAsync(int id)
        {
            var order = await _purchaseOrderRepository.GetPurchaseOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpPost("AddPurchaseOrder")]
        public async Task<IActionResult> AddPurchaseOrderAsync([FromBody] PurchaseOrderAddDto purchaseOrderDto)
        {
            try
            {
                await _purchaseOrderRepository.AddPurchaseOrderAsync(purchaseOrderDto);
                return Ok(purchaseOrderDto);
            }
            catch (Exception ex)
            {
                // Handle exception
                return BadRequest(ex.Message);
            }

            
        }







        [HttpPut("UpdatePurchaseOrder/{id}")]
        public async Task<ActionResult> UpdatePurchaseOrderAsync(int id, PurchaseOrderUpdateDto order)
        {
            if (id != order.Id) return BadRequest("ID mismatch.");

            try
            {
                var updatedOrder = await _purchaseOrderRepository.UpdatePurchaseOrderAsync(order);
                return Ok(updatedOrder);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception details here
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpDelete("Delete")]
        public async Task<ActionResult> DeletePurchaseOrderAsync(int id)
        {
            await _purchaseOrderRepository.DeletePurchaseOrderAsync(id);
            return Ok("Order Deleted");
        }

        [HttpPost("SearchPurchaseOrders")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> SearchPurchaseOrders([FromBody] SearchPurchaseOrderDto searchDto)
        {
            var orders = await _purchaseOrderRepository.SearchPurchaseOrdersAsync(searchDto.Status, searchDto.Date);
            return Ok(orders);
        }

    }
}
