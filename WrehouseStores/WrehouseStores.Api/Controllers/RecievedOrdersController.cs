using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.RecievedOrderDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{

    public class RecievedOrdersController : BaseApiController
    {
        private readonly IReceivedOrder _receivedOrder;

        public RecievedOrdersController(IReceivedOrder receivedOrder)
        {
            _receivedOrder = receivedOrder;
        }

        [HttpGet("AllRecievedOrders")]
        public async Task<RecievedOrderWithPaginationDto> GetAllRecievedOrdersAsync(int pageNumber = 1, int pageSize = 10)
        {
            var orders = await _receivedOrder.GetAllRecievedOrdersAsync(pageNumber, pageSize);
            return orders;
        }

        [HttpGet("AllStoredRecievedOrders")]
        public async Task<RecievedOrderWithPaginationDto> GetAllStoredReceivedOrdersAsync(int pageNumber = 1 , int pageSize = 10)
        {
            var storedOrders = await _receivedOrder.GetAllStoredOrdersAsync(pageNumber , pageSize);
            return storedOrders;
        }

        [HttpDelete("DeleteStoredOrder")]
        public async Task<ActionResult> DeleteStoreOrderAsync(int id)
        {
           await _receivedOrder.DeleteStoredOrderAsync(id);
            return Ok("Order Deleted");
        }

        [HttpPost("SearchRecievedOrders")]
        public async Task<ActionResult<IEnumerable<RecievedOrderDto>>> SerachRecievedOrdersAsync([FromQuery]DateTime? date,[FromBody] RecievedOrderSearchRequestDto requestDto)
        {
            var orders = await _receivedOrder.SearchRecievedOrdersAsync(date , requestDto.OrderId);
            if(!orders.Any()) return NotFound();

            return Ok(orders);
        }

        [HttpPost("SearchStoredOrders")]
        public async Task<ActionResult<IEnumerable<RecievedOrderDto>>> SerachStoredOrdersAsync(DateTime? date ,[FromBody]StoredOrderSearchRequestDto requestDto)
        {
            var orders = await _receivedOrder.SearchStoredOrdersAsync( date , requestDto.SearchText);
            if (!orders.Any()) return NotFound();

            return Ok(orders);
        }
        [HttpGet("RecievedOrderById{id}")]
        public async Task<ActionResult> GetRecievedOrderById(int id)
        {
            var order = await _receivedOrder.GetRecivedOrderByIdAsync(id);

            //return Ok("Order Deleted");
            return Ok(order);
        }

    }
}
