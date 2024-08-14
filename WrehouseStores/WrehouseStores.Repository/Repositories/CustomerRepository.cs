using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.CustomerDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly StorageDbContext _context;

        public CustomerRepository(StorageDbContext context)
        {
            _context = context;
        }
        public async Task AddCustomerAsync(CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            var customer = new Customers
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                Phone = customerDto.Phone,
                CommercialRegister = customerDto.CommercialRegister,
                Address = customerDto.Address,
                Email = customerDto.Email,
            };

            _context.Add(customer);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer is not null)
            {
                 _context.Remove(customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("العميل غير متواجد");
            }

        }
        public async Task<CustomerWithPaginationDto> GatAllCustomersAsync(int pageNumber, int pageSize)
        {
            var query = _context.Customers;
            int totalCustomers = await query.CountAsync();
            List<Customers> customers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new CustomerWithPaginationDto
            {
                Customers = customers.Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    Email = c.Email,
                    CommercialRegister = c.CommercialRegister,
                    Phone = c.Phone
                }).ToList(),
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Total = totalCustomers,
                    HasNextPage = pageNumber * pageSize < totalCustomers,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return response;
        }
        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
           var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer is null) throw new ArgumentException("العميل غير متواجد");

            var customerDetails = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Address = customer.Address,
                Email = customer.Email,
                CommercialRegister = customer.CommercialRegister,
            };
            return customerDetails;




        }
        public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string? name = null)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(C => C.Name.Contains(name));

            var customers = await query.Select(C => new CustomerDto
            {
                Id = C.Id,
                Name = C.Name,
                Phone = C.Phone,
                Address = C.Address,
                Email = C.Email,
                CommercialRegister = C.CommercialRegister,
            }).ToListAsync();

            return customers;
        }


        public async Task UpdateCustomerAsync(CustomerDto customerDto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerDto.Id);

            if (customer is not null)
            {
                customer.Name = customerDto.Name;
                customer.Phone = customerDto.Phone;
                customer.Email = customerDto.Email;
                customer.Address = customerDto.Address;
                customer.CommercialRegister = customerDto.CommercialRegister;

                await _context.SaveChangesAsync();
            }

            _context.Update(customer);
            
           await _context.SaveChangesAsync();


        }
    }
}
