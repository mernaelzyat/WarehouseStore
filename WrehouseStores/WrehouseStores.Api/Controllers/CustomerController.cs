using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto.CustomerDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{

    public class CustomerController : BaseApiController
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("AllCustomers")]
        public async Task<CustomerWithPaginationDto> GetAllCustomersAsync(int pageNumber = 1, int pageSize = 10)
        {
            var customers = await _customerRepository.GatAllCustomersAsync(pageNumber, pageSize);
            return customers;
        }

        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("CustomerDto cannot be null.");
            }

            try
            {
                await _customerRepository.AddCustomerAsync(customerDto); // Assuming the service method name is AddCustomerAsync
                return Ok("Customer added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

         [HttpPut("UpdateCustomer")]
        public async Task<ActionResult> UpdateCustomerAsync(int id , CustomerDto customerDto)
        {
            if (id != customerDto.Id) return BadRequest();

            await _customerRepository.UpdateCustomerAsync(customerDto);
            return Ok(customerDto);
        }

        [HttpGet("CustomerById")]
        public async Task<ActionResult> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return Ok(customer);
        }
        [HttpDelete("DeleteCustomer")]
        public async Task<ActionResult> DeleteCustomerAsync(int id )
        {
            await _customerRepository.DeleteCustomerAsync(id);
            return Ok("Customer deleted");
        }

        [HttpPost("SearchCustomer")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomerAsync([FromBody] CustomerSearchRequestDto searchDto)
        {
            var customers = await _customerRepository.SearchCustomersAsync(searchDto.Name);
            if (!customers.Any()) return NotFound();

            return Ok(customers);
        }



    }
}
