using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly StorageDbContext _context;

        public StatusRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PriorityDto>> GetAllPriorityAsync()
        {
            var priority = await _context.Priority.Select(p => new PriorityDto
            {
                Id = p.Id,
                Name = p.Name,
            }).ToListAsync();

            return priority;
        }

        public async Task<IEnumerable<StatusDto>> GetAllStatusAsync()
        {
            var status = await _context.Status.Select(s => new StatusDto
            {
                Id = s.Id,
                Name = s.Name,
            }).ToListAsync();

            return status;
        }
    }
}
