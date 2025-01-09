using ModularPatternTraining.Modules.BookModule.DTO;
using ModularPatternTraining.Modules.BookModule.Models;

namespace ModularPatternTraining.Modules.BookModule.Dto
{
    public abstract class BookMap
    {
        public static BookDTO ToDto(Book book)
        {
            return new BookDTO()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Description = book.Description,
                Title = book.Title
            };
        }

        public static Book ToEntity(BookDTO bookDto)
        {
            return new Book()
            {
                Id = bookDto.Id,
                Name = bookDto.Name,
                Author = bookDto.Author,
                Description = bookDto.Description,
                Title = bookDto.Title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
