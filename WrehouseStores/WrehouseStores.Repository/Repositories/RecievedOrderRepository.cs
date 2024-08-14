using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.BillDtos;
using WarehouseStores.Core.Dto.CategoryDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.PurchaseOrderDtos;
using WarehouseStores.Core.Dto.RecievedOrderDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WarehouseStores.Repository.Repositories
{
    public class RecievedOrderRepository : IReceivedOrder
    {
        private readonly StorageDbContext _context;

        public RecievedOrderRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task DeleteStoredOrderAsync(int id)
        {
            var storeOrder = await _context.ReceivedOrders.FindAsync(id);
            if (storeOrder is not null)
            {
                _context.ReceivedOrders.Remove(storeOrder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RecievedOrderWithPaginationDto> GetAllRecievedOrdersAsync(int pageNumber, int pageSize)
        {
            var totalOrders = await _context.ReceivedOrders.CountAsync();
            var orders = await _context.ReceivedOrders
                .Include(R => R.PurchaseOrder)
                .OrderBy(R => R.Id)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                .Select(R => new RecievedOrderDto
                {
                    Id = R.Id,
                    Date = R.Date,
                    Time = R.Time,
                    Notes= R.Notes,
                    PurchaseOrderDto = new PurchaseOrderDto
                    {
                        Id= R.Id,

                    }

                }).ToListAsync();
            var response = new RecievedOrderWithPaginationDto
            {
                RecievedOrders = orders,
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

        public async Task<RecievedOrderWithPaginationDto> GetAllStoredOrdersAsync(int pageNumber, int pageSize)
        {
            var totalOrders = await _context.ReceivedOrders.CountAsync();
            var storedOrders = await _context.ReceivedOrders
                .Include(so => so.Products)
                .ThenInclude(p => p.ProductDates)

                .Include(so => so.Bill)
                .Include(so => so.Department)
                .OrderBy(so => so.Id)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                .Select(so => new RecievedOrderDto
                {
                    Id = so.Id,
                   // Weight = so.Weight,
                    Products = so.Products.Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Quantity = p.Quantity,
                        Unit = p.Unit,
                        //ExpiryDate = p.ExpiryDate,
                        Weight = p.Weight,
                        ProductDatesDto = p.ProductDates.Select(pd => new ProductDatesDto
                        {
                            AddDate = pd.AddDate,
                            ExpiryDate = pd.ExpiryDate
                        }).ToList()
                    }).ToList(),
                    Bill = new BillDto
                    {
                        Id = so.Id
                    },

                    Department = new DepartmentDto
                    {
                        Name = so.Department.Name
                    }
                }).ToListAsync();



            var response = new RecievedOrderWithPaginationDto
            {
                RecievedOrders = storedOrders,
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

        public async Task<RecievedOrderWithStorageDto> GetRecivedOrderByIdAsync(int id)
        {
            var order = await _context.ReceivedOrders
                .Include(o => o.Products)
                .ThenInclude(p => p.ProductDates)
                .Include(o => o.Products)
                .ThenInclude(p => p.Category)
                .Include(o => o.Department)
                .AsSplitQuery()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            var productIds = order.Products.Select(p => p.CategoryId).ToList();
            var categories = await _context.Categories
                .Where(c => productIds.Contains(c.Id))
                .Include(c => c.Storages)
                .ToListAsync();

            var orderDto = new RecievedOrderWithStorageDto
            {
                Id = order.Id,
                Name = order.Name,
                Date = order.Date,
                Time = order.Time,
                Notes = order.Notes,
                Phone = order.Phone,
                Email = order.Email,
                DepartmentName = order.Department?.Name,
                Products = order.Products.Select(p => new ProductWithStorageDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Unit = p.Unit,
                    Weight = p.Weight,
                    ProductDatesDto = p.ProductDates?.Select(pd => new ProductDatesDto
                    {
                        AddDate = pd.AddDate,
                        ExpiryDate = pd.ExpiryDate
                    }).ToList(),
                    Category = p.Category != null ? new SimpleCategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    } : null,
                    StorageName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Storages?.FirstOrDefault()?.Name
                }).ToList()
            };

            return orderDto;
        }
        public async Task<IEnumerable<RecievedOrderDto>> SearchRecievedOrdersAsync(DateTime? date = null , int? OrderId = null)
        {
            var query =  _context.ReceivedOrders.Include(O => O.PurchaseOrder).AsQueryable();

            if (OrderId.HasValue)
                query = query.Where(O => O.Id.Value == OrderId.Value);
            if(date.HasValue)
                query = query.Where(O => O.Date.Value == date.Value);

            var orders = await query.Select(O => new RecievedOrderDto
            {
                Id = O.Id,
                Date = O.Date,
                Time = O.Time,
                Notes = O.Notes,
                PurchaseOrderDto = new PurchaseOrderDto 
                {
                    Id = O.Id,
                }


            }).ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<RecievedOrderDto>> SearchStoredOrdersAsync(DateTime? expiryDate = null, string? searchText = null)
        {
            var query = _context.ReceivedOrders
                .Include(O => O.Department)
                .Include(O => O.Bill)
                .Include(O => O.Products)
                .ThenInclude(p => p.ProductDates) // Include product dates
                .AsQueryable();

            if (expiryDate.HasValue)
            {
                query = query.Where(O =>
                    O.Products.Any(p => p.ProductDates.Any(pd => pd.ExpiryDate.Value.Date == expiryDate.Value.Date))
                );
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(O =>
                    O.Department.Name.Contains(searchText) ||
                    O.Products.Any(p => p.Name.Contains(searchText) || p.Id.ToString().Contains(searchText))
                );
            }

            var orders = await query.Select(O => new RecievedOrderDto
            {
                Id = O.Id,
                Date = O.Date,
                Department = new DepartmentDto
                {
                    Id = O.Department.Id,
                    Name = O.Department.Name
                },
                Bill = new BillDto
                {
                    Id = O.Bill.Id
                },
                Products = O.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = p.Weight,
                    ProductDatesDto = p.ProductDates.Select(pd => new ProductDatesDto
                    {
                        ExpiryDate = pd.ExpiryDate
                    }).ToList(),
                }).ToList()
            }).ToListAsync();

            return orders;
        }




    }
}
