using ModularPatternTraining.Data.AppDbContext;
using ModularPatternTraining.Modules.LibraryModule.DataAccess;
using ModularPatternTraining.Modules.LibraryModule.Dto;
using ModularPatternTraining.Shared.Models;

namespace ModularPatternTraining.Modules.LibraryModule.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _libraryRepository;

        public LibraryService(AppDbContext appDbContext, ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<Result<LibraryDto>> GetById(int id)
        {
            var library = await _libraryRepository.GetByIdAsync(id);

            return library != null
                ? Result<LibraryDto>.Success(LibraryMap.ToDto(library))
                : Result<LibraryDto>.Failure("Library Not Found", 404);
        }

        public async Task<Result<IEnumerable<LibraryDto>>> GetAllAsync()
        {
            var books = (await _libraryRepository.GetAllAsync())
                
                .Take(50)
                .Select(LibraryMap.ToDto)
                .ToList();

            return books.Count == 0
                ? Result<IEnumerable<LibraryDto>>.Failure("No Library Founds", 404)
                : Result<IEnumerable<LibraryDto>>.Success(books);
        }

        public async Task<Result<bool>> AddAsync(LibraryDto entity)
        {
            var isExist = await _libraryRepository.IsExist(entity.Name);

            if (isExist) return Result<bool>.Failure("BookExist", 400);

            await _libraryRepository.SafeSave(async () =>
            {
                await _libraryRepository.AddAsync(LibraryMap.ToEntity(entity));
            });
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateAsync(LibraryDto entity)
        {

            var exist = await _libraryRepository.IsExistById(entity.Id);
            if (!exist) return Result<bool>.Failure("Book Not Found", 404);
            var library = await _libraryRepository.GetByIdAsync(entity.Id);
            if (library == null) return Result<bool>.Failure("Library Not Found", 404);


            await _libraryRepository.SafeSave(async () =>
            {
                library.Name = entity.Name;
                library.State = entity.State;
                library.City = entity.City;
                library.description = entity.description;
                library.UpdateDateTime = DateTime.UtcNow;

            });
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var exist = await _libraryRepository.IsExistById(id);
            var removed = await _libraryRepository.IsRemoved(id);
            if (removed || !exist) return Result<bool>.Failure("Library Not Found", 404);

            var library = await _libraryRepository.GetByIdAsync(id);
            library.IsRemoved = true;
            await _libraryRepository.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<LibraryDto>> GetByNameAsync(string name)
        {
            var library = await _libraryRepository.GetByNameAsync(name);
            return library == null
                ? Result<LibraryDto>.Failure("Library Not Found", 404)
                : Result<LibraryDto>.Success(LibraryMap.ToDto(library));
        }
    }
}