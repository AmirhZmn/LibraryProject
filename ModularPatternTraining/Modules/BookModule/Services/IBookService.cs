using ModularPatternTraining.Modules.BookModule.DTO;
using ModularPatternTraining.Shared.Repositories;

namespace ModularPatternTraining.Modules.BookModule.Services
{
    public interface IBookService : IServiceRepository<BookDTO>
    {
      
    }
}
