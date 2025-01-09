using ModularPatternTraining.Data.AppDbContext;
using ModularPatternTraining.Modules.BookModule.Models;
using ModularPatternTraining.Shared.DataAccess;

namespace ModularPatternTraining.Modules.BookModule.DataAccess
{
    public class BookRepository : DataRepository<Book>,IBookRepository
    {
        public BookRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
