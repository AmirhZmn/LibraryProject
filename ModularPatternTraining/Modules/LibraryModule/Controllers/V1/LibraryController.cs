using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularPatternTraining.Modules.LibraryModule.Dto;
using ModularPatternTraining.Modules.LibraryModule.Services;
using ModularPatternTraining.Shared.Services.CacheService;

namespace ModularPatternTraining.Modules.LibraryModule.Controllers.V1
{
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase

    {
        private readonly ILibraryService _libraryService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(ILibraryService libraryService, ILogger<LibraryController> logger, ICacheService cacheService)
        {
            _libraryService = libraryService;
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpGet("AllLibraries")]
        public async Task<IActionResult> GetAllLibraries()
        {
            var libraries = await _libraryService.GetAllAsync();
            if (!libraries.IsSuccess)
            {
                return NotFound(libraries.ErrorMessage);
            }
            _logger.LogInformation("User Read The All Libraries");
            return Ok(libraries.Data);
        }

        [HttpGet("LibraryById")]
        public async Task<IActionResult> GetLibraryById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var cacheKey = $"Library_Id:{id}";
            var cachedData = _cacheService.Get<LibraryDto>(cacheKey);
            if (cachedData != null)
            {
                return Ok(cachedData);
            }
            var library =await _libraryService.GetById(id);
            if (!library.IsSuccess) return NotFound(library.ErrorMessage);
            _cacheService.Set(cacheKey,library.Data, TimeSpan.FromMinutes(10));
            return Ok(library.Data);
        }

        [HttpGet("libraryByName")]
        public async Task<IActionResult> GetLibraryByName(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var cacheKey = $"Library_Name:{name}";
            var cachedData = _cacheService.Get<LibraryDto>(cacheKey);
            if (cachedData != null)
            {
                return Ok(cachedData);
            }
            var library =await _libraryService.GetByNameAsync(name);
            if (!library.IsSuccess)return NotFound(library.ErrorMessage);
            _cacheService.Set(cacheKey, library.Data, TimeSpan.FromMinutes(10));
            return Ok(library.Data);
        }

        [HttpPost("CreateLibrary")]
        public async Task<IActionResult> CreateLibrary([FromBody]LibraryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result =  await _libraryService.AddAsync(model);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);

        }

        [HttpPost("UpdateLibrary")]
        public async Task<IActionResult> UpdateLibrary([FromBody] LibraryDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _libraryService.UpdateAsync(model);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpGet("DeleteLibrary")]
        public async Task<IActionResult> DeleteLibrary(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var library =await _libraryService.DeleteAsync(id);
            if (!library.IsSuccess) return NotFound(library.ErrorMessage);
            return Ok(library.Data);
        }


    }
}
