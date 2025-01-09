using ModularPatternTraining.Modules.BookModule.Models;
using ModularPatternTraining.Shared.DataAccess;

namespace ModularPatternTraining.Modules.BookModule.DataAccess
{
    public interface IBookRepository : IDataRepository<Book>
    {
    }
}
