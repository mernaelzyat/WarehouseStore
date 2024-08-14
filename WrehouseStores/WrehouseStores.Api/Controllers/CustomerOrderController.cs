using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.CustomerOrderDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Repository.Repositories;

namespace WarehouseStores.Api.Controllers
{

    public class CustomerOrderController : BaseApiController
    {
        private readonly ICustomerOrdersRepository _customerOrdersRepository;

        public CustomerOrderController(ICustomerOrdersRepository customerOrdersRepository)
        {
            _customerOrdersRepository = customerOrdersRepository;
        }

        [HttpGet("AllCustomerOrders")]
        public async Task<CustomerOrderWithPaginationDto> GetAllCustomerOrdersAsync(int pageNumber = 1 , int pageSize = 10)
        {
            var orders = await _customerOrdersRepository.GetAllCustomerOrdersAsync(pageNumber , pageSize);
            return orders;
        }
        [HttpPost("AddCustomerOrder")]
        public async Task<IActionResult> AddCustomerOrderAsync([FromBody] AddCustomerOrderRequestDto requestDto)
        {
            var result = await _customerOrdersRepository.AddCustomerOrderAsync(requestDto);

            if (result == null)
            {
                return NotFound(new { Message = "Customer does not exist. Please create the customer before adding the order." });
            }

            return Ok(result);
        }


        [HttpDelete("DeleteOrder")]
        public async Task<ActionResult> DeleteCustomerOrderAsync(int id )
        {
            await _customerOrdersRepository.DeleteCustomerOrderAsync(id);
            return Ok("Order Deleted");
        }

        [HttpGet("CustomerOrderById")]
        public async Task<ActionResult> GetCustomerOrderById(int id)
        {
            var order = await _customerOrdersRepository.GetCustomerOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpPut("UpdateCustomerOrder/{id}")]
        public async Task<IActionResult> UpdateCustomerOrder(int id, [FromBody] CustomerOrderUpdateDto customerOrderDto)
        {
            if (id != customerOrderDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }

            try
            {
                await _customerOrdersRepository.UpdateCustomerOrderAsync(customerOrderDto);
                return Ok("Customer order updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        
    }

        [HttpPut("updateReadyOrder/{id}")]
        public async Task<IActionResult> UpdateReadyOrder(int id, [FromBody] ReadyOrderUpdateDto readyOrderDto)
        {
            if (id != readyOrderDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }

            try
            {
                await _customerOrdersRepository.UpdateReadyOrderAsync(readyOrderDto);
                return Ok("Ready order updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("SearchCustomerOrders")]
        public async Task<ActionResult<IEnumerable<CustomerOrdersDto>>> SearchCustomerOrdersAsync([FromQuery] DateTime? date = null, [FromBody] CustomerOrderSearchRequestDto requestDto = null)
        {
            var status = requestDto?.Status;
            var orders = await _customerOrdersRepository.SearchCustomerOrdersAsync(date, status);

            if (!orders.Any()) return NotFound();

            return Ok(orders);
        }



        [HttpPost("SearchReadyOrders")]
        public async Task<ActionResult<IEnumerable<ReadyOrdersDto>>> SearchReadyOrdersAsync([FromQuery]DateTime? date ,[FromBody] CustomerOrderSearchRequestDto requestDto )
        {
            var orders = await _customerOrdersRepository.SearchReadyOrdersAsync(date, requestDto.Status);
            if(!orders.Any()) return NotFound();

            return Ok(orders);
        }

        [HttpGet("AllReadyOrders")]
        public async Task<ActionResult<ReadyOrdersWithPaginationDto>> GetAllReadyOrdersAsync(int pageNumber = 1, int pageSize = 10)
        {
            var orders = await _customerOrdersRepository.GetAllReadyOrdersAsync(pageNumber, pageSize);
            return Ok(orders);

        }

        [HttpGet("ReadyOrderById")]
        public async Task<ActionResult<ReadyOrdersDto>> GetReadyOrderById(int id)
        {
            var order = await _customerOrdersRepository.GetReadyOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpPost("PrepareCustomerOrder")]
        public async Task<ActionResult<AddReadyOrderDto>> AddReadyOrderAsync(AddReadyOrderDto addReadyOrder)
        {
            var result = await _customerOrdersRepository.AddReadyOrderAsync(addReadyOrder);

            return Ok(result);
        }

        [HttpDelete("DeleteReadyOrder")]
        public async Task<ActionResult> DeleteReadyOrderAsync(int id)
        {
            await _customerOrdersRepository.DeleteReadyOrderAsync(id);
            return Ok("Order Deleted");
        }
    }
}
