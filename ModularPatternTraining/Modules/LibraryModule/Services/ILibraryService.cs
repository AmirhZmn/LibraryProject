using ModularPatternTraining.Modules.LibraryModule.Dto;
using ModularPatternTraining.Shared.Models;
using ModularPatternTraining.Shared.Repositories;

namespace ModularPatternTraining.Modules.LibraryModule.Services
{
    public interface ILibraryService : IServiceRepository<LibraryDto>
    {
        Task<Result<LibraryDto>> GetByNameAsync(string name);
            

       
    }
}
