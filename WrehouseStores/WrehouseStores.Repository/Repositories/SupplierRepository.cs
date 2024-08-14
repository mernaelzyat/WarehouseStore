using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.SupplierDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class SupplierRepository : ISuppliersRepository
    {
        private readonly StorageDbContext _context;

        public SupplierRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task AddSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = new Suppliers
            {
                Id = supplierDto.Id,
                Name = supplierDto.Name,
                Phone = supplierDto.Phone,
                Address = supplierDto.Address,
                Email = supplierDto.Email,
            };
            await _context.AddAsync(supplier);
            await _context.SaveChangesAsync();

            
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<SupplierWithPaginationDto> GatAllSuppliersAsync(int pageNumber, int pageSize)
        {
            var totalSuppliers = await _context.Suppliers.CountAsync();
            var suppliers = await _context.Suppliers
                .OrderBy(S => S.Id)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                .Select( S => new SupplierDto
                {
                    Id = S.Id,
                    Name = S.Name,
                    Phone = S.Phone,
                    Address = S.Address,
                    Email = S.Email,
                }).ToListAsync();

            var response = new SupplierWithPaginationDto
            {
                Suppliers = suppliers,
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Total = totalSuppliers,
                    HasNextPage = (pageNumber * pageSize) < totalSuppliers,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return response;
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(S => S.Id == id);

            if (supplier == null) return null;

            var supplierDetails = new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Phone = supplier.Phone,
                Address = supplier.Address,
                Email = supplier.Email,

            };

            return supplierDetails;

        }

        public async Task<IEnumerable<SupplierDto>> SearchSuppliersAsync(string? name = null)
        {
            var query = _context.Suppliers.AsQueryable();

            if(!string.IsNullOrEmpty(name))
                query = query.Where(s => s.Name.Contains(name));

            var Suppliers = await query.Select(S => new SupplierDto
            {
                Id = S.Id,
                Name = S.Name,
                Phone = S.Phone,
                Address = S.Address,
                Email = S.Email,
            }).ToListAsync();

            return Suppliers;
        }

        public async Task UpdateSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(S => S.Id == supplierDto.Id);


            if (supplier != null)
            {
                supplier.Name = supplierDto.Name;
                supplier.Phone = supplierDto.Phone;
                supplier.Address = supplierDto.Address;
                supplier.Email = supplierDto.Email;
                supplier.Id = supplierDto.Id;

                await _context.SaveChangesAsync();
            }
        }
    }
}
