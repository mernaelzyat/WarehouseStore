using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.CustomerOrderDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
//using WarehouseStores.Repository.Data.Migrations;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class CustomerOrderRepository : ICustomerOrdersRepository
    {
        private readonly StorageDbContext _context;
        private readonly ILogger<CustomerOrderRepository> _logger;

        public CustomerOrderRepository(StorageDbContext context, ILogger<CustomerOrderRepository> logger)
        {

            _context = context;
            _logger = logger;
        }
        public async Task UpdateCustomerOrderAsync(CustomerOrderUpdateDto customerOrderDto)
        {
            var orderToUpdate = await _context.CustomerOrders
                .Include(co => co.Department)
                .Include(co => co.Status)
                .Include(co => co.CustomerOrdersProducts)
                .ThenInclude(cop => cop.Product)
                .FirstOrDefaultAsync(co => co.Id == customerOrderDto.Id);

            if (orderToUpdate == null)
            {
                throw new ArgumentException($"الطلبية {customerOrderDto.Id} غير متواجدة.");
            }

            if (orderToUpdate.StatusId != 1)
            {
                throw new ArgumentException($"CustomerOrder status is not valid for update. Current StatusId: {orderToUpdate.StatusId}");
            }

            // Update basic properties
            orderToUpdate.Name = customerOrderDto.Name;
            orderToUpdate.Time = customerOrderDto.Time;
            orderToUpdate.Email = customerOrderDto.Email;
            orderToUpdate.Phone = customerOrderDto.Phone;
            orderToUpdate.AvailabilityDate = customerOrderDto.AvailabilityDate;
            orderToUpdate.Notes = customerOrderDto.Notes;

            // Update Department
            if (!string.IsNullOrEmpty(customerOrderDto.DepartmentName))
            {
                var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Name == customerOrderDto.DepartmentName);
                if (existingDepartment != null)
                {
                    orderToUpdate.DepartmentId = existingDepartment.Id;
                    orderToUpdate.Department = existingDepartment;
                }
                else
                {
                    throw new ArgumentException($"القسم {customerOrderDto.DepartmentName} غير متواجد.");
                }
            }
            else if (customerOrderDto.DepartmentName != null)
            {
                orderToUpdate.Department.Name = customerOrderDto.DepartmentName;
                orderToUpdate.Department = await _context.Departments.FindAsync(customerOrderDto.DepartmentName);
            }

            // Update Status
            if (!string.IsNullOrEmpty(customerOrderDto.StatusName))
            {
                var existingStatus = await _context.Status.FirstOrDefaultAsync(s => s.Name == customerOrderDto.StatusName);
                if (existingStatus != null)
                {
                    orderToUpdate.StatusId = existingStatus.Id;
                    orderToUpdate.Status = existingStatus;
                }
                else
                {
                    throw new ArgumentException($"الحالة {customerOrderDto.StatusName} غير متواجدة.");
                }
            }

            // Update Products relationship
            if (customerOrderDto.Products != null && customerOrderDto.Products.Any())
            {
                // Clear existing products in the order
                _context.CustomerOrderProducts.RemoveRange(orderToUpdate.CustomerOrdersProducts);

                foreach (var productDto in customerOrderDto.Products)
                {
                    var product = await _context.Products.FindAsync(productDto.Id);
                    if (product == null)
                    {
                        throw new ArgumentException($"المنتج {productDto.Id} غير متواجد.");
                    }

                    // Add the product to the order with specified quantity and weight
                    orderToUpdate.CustomerOrdersProducts.Add(new CustomerOrdersProducts
                    {
                        CustomerOrderId = orderToUpdate.Id,
                        ProductId = productDto.Id,
                        Quantity = productDto.Quantity,
                        Weight = productDto.Weight,
                        Unit = productDto.Unit
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<CustomerOrdersDto> AddCustomerOrderAsync(AddCustomerOrderRequestDto requestDto)
        {
            // Retrieve the status and department based on names from the request DTO
            var status = await _context.Status.FirstOrDefaultAsync(s => s.Name == requestDto.StatusName);
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == requestDto.DepartmentName);

            if (status == null || department == null)
            {
                throw new ArgumentException(" القسم او الحالة غير صحيح.");
            }

            // Check if the customer already exists based on email, phone, or name
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == requestDto.Email || c.Phone == requestDto.Phone || c.Name == requestDto.Name);

            if (existingCustomer == null)
            {
                throw new ArgumentException("العميل غير موجود من فضلك اضف عميل جديد قبل اضافةالطلبية.");
            }

            // Ensure Products are not null and contain valid data
            if (requestDto.Products == null || !requestDto.Products.Any())
            {
                throw new ArgumentException("من فضلك تأكد من ادخال المنتج.");
            }

            // Create the customer order with products
            var customerOrder = new CustomerOrders
            {
                Name = requestDto.Name,
                AvailabilityDate = requestDto.AvailabilityDate,
                Time = requestDto.Time,
                Notes = requestDto.Notes,
                Phone = requestDto.Phone,
                Email = requestDto.Email,
                StatusId = status.Id,
                DepartmentId = department.Id,
                CustomerOrdersProducts = requestDto.Products.Select(p => new CustomerOrdersProducts
                {
                    ProductId = p.Id,
                    Quantity = p.Quantity,
                    Unit = p.Unit,
                    Weight = p.Weight
                }).ToList()
            };

            try
            {
                _context.CustomerOrders.Add(customerOrder);
                await _context.SaveChangesAsync(); // Save the customer order
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;
            }

            // Map the customer order to DTO
            var customerOrderDto = new CustomerOrdersDto
            {
                Id = customerOrder.Id,
                Name = customerOrder.Name,
                AvailabilityDate = customerOrder.AvailabilityDate,
                Time = customerOrder.Time,
                Notes = customerOrder.Notes,
                Phone = customerOrder.Phone,
                Email = customerOrder.Email,
                DepartmentDto = new DepartmentDto
                {
                    Name = department.Name
                },
                StatusDto = new StatusDto
                {
                    Name = status.Name
                },
                Products = customerOrder.CustomerOrdersProducts.Select(cop => new ProductDto
                {
                    Id = cop.ProductId,
                    Name = cop.Product?.Name, 
                    Quantity = cop.Quantity,
                    Unit = cop.Unit,
                    Weight = cop.Weight
                }).ToList()
            };

            return customerOrderDto;
        }


        public async Task DeleteCustomerOrderAsync(int id)
        {
            var customerOrder = await _context.CustomerOrders
                .Include(co => co.CustomerOrdersProducts)
                .FirstOrDefaultAsync(co => co.Id == id);

            if (customerOrder == null)
            {
                throw new ArgumentException("الطلبية غير متواجدة.");
            }

            if (customerOrder.StatusId != 1)
            {
                throw new InvalidOperationException($"CustomerOrder with Id {customerOrder.Id} cannot be Deleted because its status is not 1.");
            }
            // Delete related products first
            _context.CustomerOrderProducts.RemoveRange(customerOrder.CustomerOrdersProducts);

            // Delete the customer order
            _context.CustomerOrders.Remove(customerOrder);

            await _context.SaveChangesAsync();
        }

        public async Task<CustomerOrderWithPaginationDto> GetAllCustomerOrdersAsync(int pageNumber , int pageSize)
        {
            var totalOrders = await _context.CustomerOrders.CountAsync();

            var customerOrders = await _context.CustomerOrders
                .Include(co => co.Status)
                .Include(co => co.CustomerOrdersProducts)
                .OrderBy(co => co.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(co => new CustomerOrdersDto
                {
                    Id = co.Id,
                    AvailabilityDate = co.AvailabilityDate,
                    Time = co.Time,
                    Notes = co.Notes,
                    Email = co.Email,
                    Phone = co.Phone,
                    Name = co.Name,
                    StatusDto = new StatusDto
                    {
                        Name = co.Status.Name
                    }
                }).ToListAsync();

            var response = new CustomerOrderWithPaginationDto
            {
                CustomerOrders = customerOrders,
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Total = totalOrders,
                    HasNextPage = (pageNumber * pageSize) < totalOrders,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return response;
        }



        public async Task<ReadyOrdersWithPaginationDto> GetAllReadyOrdersAsync(int pageNumber , int pageSize)
        {
            var totalOrders = await _context.ReadyOrders.CountAsync();
            var readyOrders = await _context.ReadyOrders
                .Include(co => co.Status)
                .Include(co => co.CustomerOrders)
                 .OrderBy(co => co.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(co => new ReadyOrdersDto
                {
                    Id = co.Id,
                    Time = co.Time,
                    Date = co.Date,
                    Notes = co.Notes,
                    StatusDto = new StatusDto
                    {
                        Name = co.Status.Name
                    },
                    CustomerOrdersDto = new CustomerOrdersDto
                    {
                        Id = co.Id
                    }


                }).ToListAsync();

            var response = new ReadyOrdersWithPaginationDto
            {
                ReadyOrders = readyOrders,
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Total = totalOrders,
                    HasNextPage = (pageNumber * pageSize) < totalOrders,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return response;
        }
        public async Task<ReadyOrdersDto> GetReadyOrderByIdAsync(int id)
        {
            try
            {
                var readyOrder = await _context.ReadyOrders
                    .Include(o => o.CustomerOrders)
                    .Include(o => o.Status)
                    .Include(o => o.ReadyOrderProducts)
                        .ThenInclude(rop => rop.Product)
                    .Include(o => o.Department)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (readyOrder == null) return null;

                var orderDetails = new ReadyOrdersDto
                {

                    Id = readyOrder.Id,
                    Name = readyOrder.Name,
                    Notes = readyOrder.Notes,
                    Date = readyOrder.Date,
                    Time = readyOrder.Time,
                    Phone = readyOrder.Phone,
                    Email = readyOrder.Email,
                    DepartmentDto = new DepartmentDto
                    {
                        Name = readyOrder.Department.Name,
                    },
                    StatusDto = new StatusDto
                    {
                        Name = readyOrder.Status.Name,
                    },
                    ProductDtos = readyOrder.ReadyOrderProducts.Select(rop => new ProductDto
                    {
                        Id = rop.Product.Id,
                        Name = rop.Product.Name,
                        Quantity = rop.Quantity,
                        Unit = rop.Unit,
                        Weight = rop.Weight,
                        Notes = rop.Product.Notes,
                    }).ToList()
                };

                return orderDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the ready order by ID.");
                throw;
            }
        }
        public async Task<CustomerOrdersDto> GetCustomerOrderByIdAsync(int id)
        {
            var customerOrder = await _context.CustomerOrders
                .Include(co => co.Status)
                .Include(co => co.CustomerOrdersProducts)
                    .ThenInclude(cop => cop.Product) // Include Product details
                .Include(co => co.Department)
                .FirstOrDefaultAsync(co => co.Id == id);

            if (customerOrder == null) throw new ArgumentException(".الطلبية غير متواجدة");

            var orderDetails = new CustomerOrdersDto
            {
                Id = customerOrder.Id,
                Name = customerOrder.Name,
                AvailabilityDate = customerOrder.AvailabilityDate,
                Time = customerOrder.Time,
                Notes = customerOrder.Notes,
                Email = customerOrder.Email,
                Phone = customerOrder.Phone,
                DepartmentDto = new DepartmentDto
                {
                    Name = customerOrder.Department.Name
                },
                StatusDto = new StatusDto
                {
                    Name = customerOrder.Status.Name
                },
                Products = customerOrder.CustomerOrdersProducts.Select(cop => new ProductDto
                {
                    Id = cop.Product.Id,
                    Name = cop.Product.Name,
                    Quantity = cop.Quantity,
                    Unit = cop.Unit,
                    Weight = cop.Weight
                }).ToList()
            };

            return orderDetails;
        }
        public async Task<IEnumerable<CustomerOrdersDto>> SearchCustomerOrdersAsync(DateTime? availabilityDate = null, string? status = null)
        {
            var query = _context.CustomerOrders.Include(CO => CO.Status).AsQueryable();

            if (availabilityDate.HasValue)
                query = query.Where(CO => CO.AvailabilityDate == availabilityDate.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(CO => CO.Status.Name.Contains(status));

            var orders = await query.Select(CO => new CustomerOrdersDto
            {
                Id = CO.Id,
                AvailabilityDate = CO.AvailabilityDate,
                Notes = CO.Notes,
                Time = CO.Time,
                StatusDto = new StatusDto
                {
                    Name = CO.Status.Name
                }
            }).ToListAsync();

            return orders;
        }



        public async Task<IEnumerable<ReadyOrdersDto>> SearchReadyOrdersAsync(DateTime? date = null, string? status = null)
        {
            var query = _context.ReadyOrders.Include(RO => RO.Status).AsQueryable();
            if (date.HasValue)
                query = query.Where(CO => CO.Date == date.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(RO => RO.Status.Name.Contains(status));



            var readyOrders = await query.Select(RO => new ReadyOrdersDto
            {
                Id = RO.Id,
                Date = RO.Date,
                Time = RO.Time,
                Notes = RO.Notes,
                StatusDto = new StatusDto
                {
                    Name = RO.Status.Name
                },
                CustomerOrdersDto = new CustomerOrdersDto
                {
                    Id = RO.Id,
                }
            }).ToListAsync();

            return readyOrders;



        }

        public async Task UpdateReadyOrderAsync(ReadyOrderUpdateDto readyOrderDto)
        {
            var orderToUpdate = await _context.ReadyOrders
                .Include(ro => ro.Status)
                .Include(ro => ro.Department)
                .Include(ro => ro.ReadyOrderProducts)
                    .ThenInclude(rop => rop.Product)
                .FirstOrDefaultAsync(ro => ro.Id == readyOrderDto.Id);

            if (orderToUpdate == null)
            {
                throw new ArgumentException($"الطلبية {readyOrderDto.Id} غير متواجدة.");
            }

            if (orderToUpdate.StatusId != 1)
            {
                throw new ArgumentException($"PurchaseOrder status is not valid for update. Current StatusId: {orderToUpdate.StatusId}");
            }

            // Update basic properties
            orderToUpdate.Name = readyOrderDto.Name;
            orderToUpdate.Notes = readyOrderDto.Notes;
            orderToUpdate.Date = readyOrderDto.Date;
            orderToUpdate.Time = readyOrderDto.Time;
            orderToUpdate.Email = readyOrderDto.Email;
            orderToUpdate.Phone = readyOrderDto.Phone;

            // Update department if necessary
            if (!string.IsNullOrEmpty(readyOrderDto.DepartmentName))
            {
                var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Name == readyOrderDto.DepartmentName);
                if (existingDepartment != null)
                {
                    orderToUpdate.DepartmentId = existingDepartment.Id;
                    orderToUpdate.Department = existingDepartment;
                }
                else
                {
                    throw new ArgumentException($"القسم {readyOrderDto.DepartmentName} غير متواجد.");
                }
            }

            // Update Status if necessary
            if (!string.IsNullOrEmpty(readyOrderDto.StatusName))
            {
                var existingStatus = await _context.Status.FirstOrDefaultAsync(s => s.Name == readyOrderDto.StatusName);
                if (existingStatus != null)
                {
                    orderToUpdate.StatusId = existingStatus.Id;
                    orderToUpdate.Status = existingStatus;
                }
                else
                {
                    throw new ArgumentException($"الحالة {readyOrderDto.StatusName} غير متواجدة.");
                }
            }

            // Update or replace products
            foreach (var productDto in readyOrderDto.Products)
            {
                var existingProduct = orderToUpdate.ReadyOrderProducts.FirstOrDefault(p => p.ProductId == productDto.Id);

                if (existingProduct != null)
                {
                    // Update existing product
                    existingProduct.Quantity = productDto.Quantity;
                    existingProduct.Unit = productDto.Unit;
                    existingProduct.Weight = productDto.Weight;
                    existingProduct.Product.Notes = productDto.Notes;
                }
                else
                {
                    // Add new product
                    var product = await _context.Products.FindAsync(productDto.Id);
                    if (product != null)
                    {
                        orderToUpdate.ReadyOrderProducts.Add(new ReadyOrdersProducts
                        {
                            ReadyOrderId = orderToUpdate.Id,
                            ProductId = productDto.Id,
                            Quantity = productDto.Quantity,
                            Unit = productDto.Unit,
                            Weight = productDto.Weight
                        });
                    }
                    else
                    {
                        throw new ArgumentException($"المنتج {productDto.Id} غير متواجد.");
                    }
                }
            }

            // Remove products that are no longer in the order
            var productIds = readyOrderDto.Products.Select(p => p.Id).ToList();
            var productsToRemove = orderToUpdate.ReadyOrderProducts.Where(p => !productIds.Contains((int)p.ProductId)).ToList();
            _context.ReadyOrderProducts.RemoveRange(productsToRemove);

            await _context.SaveChangesAsync();
        }


        public async Task<ReadyOrdersDto> AddReadyOrderAsync(AddReadyOrderDto addReadyOrderDto)
        {
            // Retrieve the status and department based on names from the request DTO
            var status = await _context.Status.FirstOrDefaultAsync(s => s.Name == addReadyOrderDto.StatusName);
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == addReadyOrderDto.DepartmentName);

            if (status == null || department == null)
            {
                throw new ArgumentException("تأكد من ادخال القسم والحالة بشكل صحيح.");
            }

            // Retrieve the customer order and related products
            var customerOrder = await _context.CustomerOrders
                .Include(co => co.CustomerOrdersProducts)
                .ThenInclude(cop => cop.Product)
                .FirstOrDefaultAsync(co => co.Id == addReadyOrderDto.CustomerOrderId);

            if (customerOrder == null)
            {
                throw new ArgumentException("الطلبية غير متواجدة.");
            }

            // Create and populate the ready order
            var readyOrder = new ReadyOrders
            {
                Name = customerOrder.Name,
                Phone = customerOrder.Phone,
                Email = customerOrder.Email,
                Notes = customerOrder.Notes,
                Date = DateTime.UtcNow,
                Time = DateTime.UtcNow,
                Department = department,
                Status = status,
                CustomerOrders = new List<CustomerOrders> { customerOrder },
                ReadyOrderProducts = new List<ReadyOrdersProducts>()
            };

            foreach (var productDto in addReadyOrderDto.Products)
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productDto.Id);
                if (existingProduct == null)
                {
                    throw new ArgumentException($"المنتج {productDto.Id} غير متواجد.");
                }

                var readyOrderProduct = new ReadyOrdersProducts
                {
                    ProductId = existingProduct.Id,
                    Quantity = productDto.Quantity.Value,
                    Unit = productDto.Unit,
                    Weight = productDto.Weight.Value,
                };

                readyOrder.ReadyOrderProducts.Add(readyOrderProduct);
            }

            try
            {
                _context.ReadyOrders.Add(readyOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Error occurred: {dbEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;
            }

            // Map to DTO
            var readyOrderDto = new ReadyOrdersDto
            {
                Id = readyOrder.Id,
                Name = readyOrder.Name,
                Phone = readyOrder.Phone,
                Email = readyOrder.Email,
                Notes = readyOrder.Notes,
                Date = readyOrder.Date,
                Time = readyOrder.Time,
                DepartmentDto = new DepartmentDto
                {
                    Name = department.Name
                },
                StatusDto = new StatusDto
                {
                    Name = status.Name
                },
                ProductDtos = readyOrder.ReadyOrderProducts.Select(p => new ProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    Quantity = p.Quantity,
                    Unit = p.Unit,
                    Weight = p.Weight
                }).ToList()
            };

            return readyOrderDto;
        }

        public async Task DeleteReadyOrderAsync(int id)
        {
            var readyOrder = await _context.ReadyOrders
                .Include(RO => RO.ReadyOrderProducts)
                .FirstOrDefaultAsync(RO => RO.Id == id);

            if (readyOrder == null) throw new ArgumentException("الطلبية غير متواجدة.");

            if (readyOrder.StatusId != 1)
            {
                throw new InvalidOperationException($"ReadyOrder with Id {readyOrder.Id} cannot be Deleted because its status is not 1.");
            }

            _context.ReadyOrderProducts.RemoveRange(readyOrder.ReadyOrderProducts);
            _context.Remove(readyOrder);

            await _context.SaveChangesAsync();
        }
    }
}











