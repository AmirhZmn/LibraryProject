using ModularPatternTraining.Modules.LibraryModule.Models;

namespace ModularPatternTraining.Modules.LibraryModule.Dto
{
    public abstract class LibraryMap
    {
        public static LibraryDto ToDto(Library library)
        {
            return new LibraryDto()
            {
                Id = library.Id,
                Name = library.Name,
                City = library.City, 
                State = library.State,
                description = library.description
              
            };
        }

        public static Library ToEntity(LibraryDto libraryDto)
        {
            return new Library()
            {
                Id = libraryDto.Id,
                Name = libraryDto.Name,
                description = libraryDto.description,
                State = libraryDto.State,
                City = libraryDto.City,
                AddDateTime = DateTime.UtcNow,
                UpdateDateTime = DateTime.UtcNow
            };
        }
    }
}
