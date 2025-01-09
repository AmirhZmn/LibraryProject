using ModularPatternTraining.Modules.LibraryModule.Models;
using ModularPatternTraining.Shared.DataAccess;

namespace ModularPatternTraining.Modules.LibraryModule.DataAccess
{
    public interface ILibraryRepository : IDataRepository<Library>
    {
        Task<Library> GetByNameAsync(string name);
    }
}
