using ModularPatternTraining.Shared.Models;

namespace ModularPatternTraining.Shared.Repositories
{
    public interface IServiceRepository<T> where T : class
    {
        Task<Result<T>> GetById(int id);
        Task<Result<IEnumerable<T>>> GetAllAsync();

        Task<Result<bool>> AddAsync(T entity);
        Task<Result<bool>> UpdateAsync(T entity);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
