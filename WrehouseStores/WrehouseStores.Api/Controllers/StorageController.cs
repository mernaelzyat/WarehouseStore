using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.StorageDtos;
using WarehouseStores.Core.Interfaces;

namespace WarehouseStores.Api.Controllers
{
    public class StorageController : BaseApiController
    {
        private readonly IStorageRepository _storageRepository;

        public StorageController(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }
        [HttpGet("AllStoragePagination")]
        public async Task<StorageWithPaginationDto> GetAllStoragesWithPaginationAsync(int pageNumber = 1 , int pageSize = 10)
        {
            var storages = await _storageRepository.GetAllStoragesWithPaginationAsync(pageNumber, pageSize);
            return storages;
        }

        [HttpGet("AllStorage")]
        public async Task<List<StorageDto>> GetAllStoragesAsync()
        {
            var storages = await _storageRepository.GetAllStoragesAsync();
            return storages;
        }

        [HttpPost("AddStorage")]
        public async Task<ActionResult> AddStorageAsync(StorageDto storage)
        {
            await _storageRepository.AddStorageAsync(storage);
            return Ok(storage);
        }

        [HttpPut("UpdateStorage")]
        public async Task<ActionResult> UpdateStorageAsync(int id, StorageDto storage)
        {
            if (id != storage.Id) return NotFound();

            await _storageRepository.UpdateStorageAsync(storage);
            return Ok(storage);
        }

        [HttpDelete("DeleteStorage")]
        public async Task<ActionResult> DeleteStorageAsync(int id)
        {
            await _storageRepository.DeleteStorageAsync(id);
            return Ok("Storage Deleted");

        }

        [HttpGet("SearchStorage")]
        public async Task<ActionResult> SearchStoragesAsync(string? name = null)
        {
            var storages = await _storageRepository.SearchStoragesAsync(name);
            if (!storages.Any()) return NotFound("storage not found");

            return Ok(storages);
        }
        [HttpGet("AllDepartments")]
        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _storageRepository.GetAllDepartmentsAsync();
            return departments;
        }
    }
}
