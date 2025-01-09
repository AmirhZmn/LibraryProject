using ModularPatternTraining.Modules.BookModule.DataAccess;
using ModularPatternTraining.Modules.BookModule.Dto;
using ModularPatternTraining.Modules.BookModule.DTO;
using ModularPatternTraining.Shared.Models;


namespace ModularPatternTraining.Modules.BookModule.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        public async Task<Result<BookDTO>> GetById(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book == null ? Result<BookDTO>.Failure("Book Not Found", 404) : Result<BookDTO>.Success(BookMap.ToDto(book));

        }

        public async Task<Result<IEnumerable<BookDTO>>> GetAllAsync()
        {
            var booksList = (await _bookRepository.GetAllAsync()).Select(BookMap.ToDto).ToList();
            return booksList.Count == 0 ? Result<IEnumerable<BookDTO>>.Failure("No Books Found", 404) : Result<IEnumerable<BookDTO>>.Success(booksList);

        }

        public async Task<Result<bool>> AddAsync(BookDTO entity)
        {
            var isExist = await _bookRepository.IsExist(entity.Name);

            if (isExist) return  Result<bool>.Failure("BookExist",400);
            
            var book = BookMap.ToEntity(entity);

            await _bookRepository.SafeSave(async () =>
            {
                await _bookRepository.AddAsync(book);
            });
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateAsync(BookDTO entity)
        {
            var exist = await _bookRepository.IsExistById(entity.Id);
            if (!exist) return Result<bool>.Failure("Book Not Found", 404);
            var book = await _bookRepository.GetByIdAsync(entity.Id);
            if (book == null) return Result<bool>.Failure("Book Not Found", 404);

            await _bookRepository.SafeSave(async () =>
            {
                book.Name = entity.Name;
                book.Author = entity.Author;
                book.Description = entity.Description;
                book.Title = entity.Title;
                book.UpdatedAt = DateTime.Now;
            });
            return Result<bool>.Success(true);
;
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var exist = await _bookRepository.IsExistById(id);
            if (!exist) return Result<bool>.Failure("Book Not Found", 404);
            var removed = await _bookRepository.IsRemoved(id);
            if (removed ) return Result<bool>.Failure("Book Not Found", 404);
            
            var book = await _bookRepository.GetByIdAsync(id);
            book.IsRemoved = true;
            await _bookRepository.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
