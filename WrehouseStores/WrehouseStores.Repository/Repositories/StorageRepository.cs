using Microsoft.EntityFrameworkCore;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.StorageDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly StorageDbContext _context;

        public StorageRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task<StorageResponseDto> AddStorageAsync(StorageDto storageDto)
        {
            var storage = new Storage
            {
                Id = storageDto.Id,
                Phone = storageDto.Phone,
                Location = storageDto.Location,
                Supervisor = storageDto.Supervisor,
                Name = storageDto.Name,
                StorageDepartments = new List<StorageDepartment>()
            };

            foreach (var departmentDto in storageDto.Departments)
            {
                var department = await _context.Departments.FindAsync(departmentDto.Id);
                if (department != null)
                {
                    storage.StorageDepartments.Add(new StorageDepartment
                    {
                        Department = department,
                        Storage = storage
                    });
                }
                else
                {
                    department = new Departments
                    {
                        Name = departmentDto.Name,
                    };
                    storage.StorageDepartments.Add(new StorageDepartment
                    {
                        Department = department,
                        Storage = storage
                    });
                }
            }

            await _context.Storage.AddAsync(storage);
            await _context.SaveChangesAsync();

            var response = new StorageResponseDto
            {
                Id = storage.Id,
                Phone = storage.Phone,
                Name = storage.Name,
                Location = storage.Location,
                Supervisor = storage.Supervisor,
                Departments = storageDto.Departments
            };

            return response;
        }


        public async Task DeleteStorageAsync(int id)
        {
            var storage = await _context.Storage.FindAsync(id);
            if (storage != null) 
            {
                _context.Storage.Remove(storage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _context.Departments
                .Select(D => new DepartmentDto
                {
                    Name = D.Name,
                    Id = D.Id,

                }).ToListAsync();

            return departments;
        }
        public async Task<List<StorageDto>> GetAllStoragesAsync()
        {
            var storages = await _context.Storage
                .Include(S => S.StorageDepartments)
                    .ThenInclude(sd => sd.Department)
                .Select(S => new StorageDto
                {
                    Id = S.Id,
                    Phone = S.Phone,
                    Location = S.Location,
                    Supervisor = S.Supervisor,
                    Name = S.Name,
                    Departments = S.StorageDepartments.Select(sd => new DepartmentDto
                    {
                        Id = sd.Department.Id,
                        Name = sd.Department.Name,
                    }).ToList(),

                }).ToListAsync();

            return storages;
        }

        public async Task<StorageWithPaginationDto> GetAllStoragesWithPaginationAsync(int pageNumber, int pageSize)
        {
            var totalStorages = await _context.Storage.CountAsync();
            var storages = await _context.Storage
                .Include(S => S.StorageDepartments)
                    .ThenInclude(sd => sd.Department)
                    .OrderBy(S => S.Id)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                .Select(S => new StorageDto
                {
                    Id = S.Id,
                    Phone = S.Phone,
                    Location = S.Location,
                    Supervisor = S.Supervisor,
                    Name = S.Name,
                    Departments = S.StorageDepartments.Select(sd => new DepartmentDto
                    {
                        Id = sd.Department.Id,
                        Name = sd.Department.Name,
                    }).ToList(),

                }).ToListAsync();
            var response = new StorageWithPaginationDto
            {
                Storages = storages,
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Total = totalStorages,
                    HasNextPage = (pageNumber * pageSize) < totalStorages,
                    HasPreviousPage = pageNumber > 1
                }
            };

            return response;
        }




        public async Task<IEnumerable<StorageDto>> SearchStoragesAsync(string? name = null)
        {
            var query = _context.Storage
                .Include(S => S.StorageDepartments)
                    .ThenInclude(sd => sd.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(s => s.Name.Contains(name));

            var storages = await query.Select(s => new StorageDto
            {
                Id = s.Id,
                Phone = s.Phone,
                Location = s.Location,
                Supervisor = s.Supervisor,
                Name = s.Name,
                Departments = s.StorageDepartments.Select(sd => new DepartmentDto
                {
                    Id = sd.Department.Id,
                    Name = sd.Department.Name
                }).ToList()
            }).ToListAsync();

            return storages;
        }


        public async Task UpdateStorageAsync(StorageDto storageDto)
        {
            var storage = await _context.Storage
                .Include(S => S.StorageDepartments)
                    .ThenInclude(sd => sd.Department)
                .FirstOrDefaultAsync(S => S.Id == storageDto.Id);

            if (storage == null)
                throw new ArgumentException($"المخزن {storageDto.Id} غير متواجد.");

            storage.Name = storageDto.Name;
            storage.Phone = storageDto.Phone;
            storage.Location = storageDto.Location;
            storage.Supervisor = storageDto.Supervisor;

            // Remove departments that are not in the incoming DTO
            var departmentsToRemove = storage.StorageDepartments
                .Where(sd => !storageDto.Departments.Any(d => d.Id == sd.DepartmentId))
                .ToList();

            foreach (var dept in departmentsToRemove)
            {
                storage.StorageDepartments.Remove(dept);
            }

            // Add or update departments from the incoming DTO
            foreach (var departmentDto in storageDto.Departments)
            {
                var existingDepartment = storage.StorageDepartments
                    .FirstOrDefault(sd => sd.DepartmentId == departmentDto.Id);

                if (existingDepartment == null)
                {
                    var department = await _context.Departments
                        .FirstOrDefaultAsync(d => d.Id == departmentDto.Id);

                    if (department != null)
                    {
                        storage.StorageDepartments.Add(new StorageDepartment
                        {
                            DepartmentId = department.Id,
                            StorageId = storage.Id,
                            Department = department,
                            Storage = storage
                        });
                    }
                }
            }

            _context.Update(storage);

            await _context.SaveChangesAsync();
        }

        
    }
}
