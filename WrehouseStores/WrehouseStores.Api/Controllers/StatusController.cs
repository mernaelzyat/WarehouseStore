using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{

    public class StatusController : BaseApiController
    {
        private readonly IStatusRepository _statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet("AllStatus")]
        public async Task<IEnumerable<StatusDto>> GetAllStatusAsync()
        {
            var status = await _statusRepository.GetAllStatusAsync();
            return status;
        }

        [HttpGet("AllPriority")]
        public async Task<IEnumerable<PriorityDto>> GetAllPriorityAsync()
        {
            var priorities = await _statusRepository.GetAllPriorityAsync();
            return priorities;
        }
    }
}
