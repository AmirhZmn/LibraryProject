using Microsoft.EntityFrameworkCore;
using ModularPatternTraining.Data.AppDbContext;
using ModularPatternTraining.Modules.LibraryModule.Models;
using ModularPatternTraining.Shared.DataAccess;

namespace ModularPatternTraining.Modules.LibraryModule.DataAccess
{
    public class LibraryRepository : DataRepository<Library>,ILibraryRepository
    {
        public LibraryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Library> GetByNameAsync(string name)
        {
            return await _appDbContext.Libraries
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == name);
        }
    }
}
