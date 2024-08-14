using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.CategoryDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.PurchaseOrderDtos;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Dto.StorageDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class PurchaseOrderRepository : IPurchaseRepository
    {
        private readonly StorageDbContext _context;
        private readonly IMapper _mapper;

        public PurchaseOrderRepository(StorageDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddPurchaseOrderAsync(PurchaseOrderAddDto purchaseOrderAddDto)
        {
            // Fetch related entities
            var department = await _context.Departments.SingleOrDefaultAsync(d => d.Name == purchaseOrderAddDto.DepartmentName);
            var priority = await _context.Priority.SingleOrDefaultAsync(p => p.Name == purchaseOrderAddDto.PriorityName);
            var status = await _context.Status.SingleOrDefaultAsync(s => s.Name == purchaseOrderAddDto.StatusName);

            if (department == null)
            {
                throw new Exception("القسم غير متواجد.");
            }

            if (priority == null)
            {
                throw new Exception("الاولوية غير متواجدة.");
            }

            if (status == null)
            {
                throw new Exception("الحالة غير متواجدة.");
            }

            // Create purchase order
            var purchaseOrder = new PurchaseOrders
            {
                Name = purchaseOrderAddDto.Name,
                Date = purchaseOrderAddDto.Date,
                Notes = purchaseOrderAddDto.Notes,
                Email = purchaseOrderAddDto.Email,
                Phone = purchaseOrderAddDto.Phone,
                Department = department,
                Priority = priority,
                Status = status,
                PurchaseOrderProducts = new List<PurchaseOrderProducts>()
            };

            // Add products to purchase order
            foreach (var productDto in purchaseOrderAddDto.Products)
            {
                var product = await _context.Products.FindAsync(productDto.Id);
                if (product == null)
                {
                    throw new Exception($"المنتج {productDto.Id} غير متواجد.");
                }

                purchaseOrder.PurchaseOrderProducts.Add(new PurchaseOrderProducts
                {
                    ProductId = product.Id,
                    Quantity = productDto.Quantity,
                    Weight = productDto.Weight
                });
            }

            // Add to context and save
            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

            if (purchaseOrder.StatusId != 1)
            {
                throw new InvalidOperationException($"PurchaseOrder with Id {purchaseOrder.Id} cannot be Deleted because its status is not 1.");
            }
            if (purchaseOrder != null) 
            {
                 _context.PurchaseOrders.Remove(purchaseOrder);
                 await _context.SaveChangesAsync();


            }
        }
        public async Task<PurchaseOrderWithPagination>GetAllPurchaseOrderAsync(int pageNumber, int pageSize)
        {
            var totalOrders = await _context.PurchaseOrders.CountAsync();
            var orders = await _context.PurchaseOrders
                .Include(P => P.Status)
                .Include(P => P.Priority)
                .Include(P => P.PurchaseOrderProducts)
                .Include(P => P.Department)
                .OrderBy(P => P.Id)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                .Select(P => new PurchaseOrderDto
                {
                    Id = P.Id,
                    Date = P.Date, 
                    Notes = P.Notes,
                    StatusDto = new StatusDto
                    {
                        Name = P.Status.Name
                    }

                }).ToListAsync();


            var response = new PurchaseOrderWithPagination
            {
                PurchaseOrders = orders,
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
        public async Task<PurchaseOrderDto> GetPurchaseOrderByIdAsync(int id)
        {
            var order = await _context.PurchaseOrders
                .Include(p => p.Status)
                .Include(p => p.Priority)
                .Include(p => p.Department)
                .Include(p => p.PurchaseOrderProducts)
                .ThenInclude(pop => pop.Product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (order == null) return null;

            var orderDetails = new PurchaseOrderDto
            {
                Id = order.Id,
                Name = order.Name,
                Date = order.Date,
                Phone = order.Phone,
                Email = order.Email,
                Notes = order.Notes,
                StatusDto = order.Status != null ? new StatusDto
                {
                    Id = order.Status.Id,
                    Name = order.Status.Name
                } : null,
                PriorityDto = order.Priority != null ? new PriorityDto
                {
                    Id = order.Priority.Id,
                    Name = order.Priority.Name
                } : null,
                DepartmentDto = order.Department != null ? new DepartmentDto
                {
                    Id = order.Department.Id,
                    Name = order.Department.Name
                } : null,
                Products = order.PurchaseOrderProducts != null ? order.PurchaseOrderProducts.Select(pop => new PurchaseOrderProductDto
                {
                    ProductId = pop.ProductId,
                    ProductName = pop.Product.Name,
                    Quantity = pop.Quantity,
                    Weight = pop.Weight
                }).ToList() : new List<PurchaseOrderProductDto>()
            };

            return orderDetails;
        }



        public async Task<IEnumerable<PurchaseOrderDto>> SearchPurchaseOrdersAsync(string? status = null, DateTime? date = null)
        {
            IQueryable<PurchaseOrders> query = _context.PurchaseOrders.Include(o => o.Status);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status.Name.Contains(status));

            if (date.HasValue)
                query = query.Where(o => o.Date == date.Value);

            var orders = await query.Select(o => new PurchaseOrderDto
            {
                Id = o.Id,
                Date = o.Date,
                Notes = o.Notes,
                StatusDto = new StatusDto
                {
                    Id = o.Status.Id,
                    Name = o.Status.Name
                }
            }).ToListAsync();

            return orders;
        }


        public async Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(PurchaseOrderUpdateDto updateDto)
        {
            if (updateDto == null || updateDto.Id == 0)
            {
                throw new ArgumentException("تأكد من ادخال البيانات بشكل صحيح.");
            }

            var order = await _context.PurchaseOrders
                .Include(p => p.Status)
                .Include(p => p.Priority)
                .Include(p => p.PurchaseOrderProducts)
                .ThenInclude(pop => pop.Product)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == updateDto.Id);

            if (order == null)
            {
                throw new ArgumentException($"الطلبية {updateDto.Id} غير متواجدة.");
            }

            

            // Check if the current status allows for updates
            if (order.StatusId != 1)
            {
                throw new ArgumentException($"PurchaseOrder status is not valid for update. Current StatusId: {order.StatusId}");
            }

            // Manually map the updateDto to the existing order entity
            order.Name = updateDto.Name;
            order.Date = updateDto.Date;
            order.Notes = updateDto.Notes;
            order.Email = updateDto.Email;
            order.Phone = updateDto.Phone;

            // Update department, priority, and status if names are provided
            if (!string.IsNullOrEmpty(updateDto.DepartmentName))
            {
                var department = await _context.Departments
                    .FirstOrDefaultAsync(d => d.Name == updateDto.DepartmentName);
                if (department != null)
                {
                    order.DepartmentId = department.Id;
                }
            }

            if (!string.IsNullOrEmpty(updateDto.PriorityName))
            {
                var priority = await _context.Priority
                    .FirstOrDefaultAsync(p => p.Name == updateDto.PriorityName);
                if (priority != null)
                {
                    order.PriorityId = priority.Id;
                }
            }

            if (!string.IsNullOrEmpty(updateDto.StatusName))
            {
                var status = await _context.Status
                    .FirstOrDefaultAsync(s => s.Name == updateDto.StatusName);
                if (status != null)
                {
                    order.StatusId = status.Id;
                }
                else
                {
                    throw new ArgumentException($"لا يوجد حالة: {updateDto.StatusName}");
                }
            }

            // Handle updating products
            if (updateDto.Products != null)
            {
                // Remove existing products that are not in the updateDto
                var productIds = updateDto.Products.Select(p => p.Id).ToList();
                var productsToRemove = order.PurchaseOrderProducts.Where(p => !productIds.Contains(p.ProductId)).ToList();
                foreach (var product in productsToRemove)
                {
                    order.PurchaseOrderProducts.Remove(product);
                }

                // Update or add products
                foreach (var productDto in updateDto.Products)
                {
                    var existingProduct = order.PurchaseOrderProducts.FirstOrDefault(p => p.ProductId == productDto.Id);
                    if (existingProduct != null)
                    {
                        existingProduct.Quantity = productDto.Quantity;
                        existingProduct.Weight = productDto.Weight ?? existingProduct.Weight;
                    }
                    else
                    {
                        var product = await _context.Products.FindAsync(productDto.Id);
                        if (product == null)
                        {
                            throw new Exception($"المنتج  {productDto.Id} غير متواجد.");
                        }

                        order.PurchaseOrderProducts.Add(new PurchaseOrderProducts
                        {
                            ProductId = product.Id,
                            Quantity = productDto.Quantity,
                            Weight = productDto.Weight
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderDto>(order);
        }






    }
}
