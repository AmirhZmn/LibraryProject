namespace ModularPatternTraining.Shared.DataAccess
{
    public interface IDataRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);

        Task SaveChangesAsync();

        Task SafeSave(Func<Task> dbOperations);
        Task<bool> IsExist(string name);
        Task<bool> IsExistById(int id);
        Task<bool> IsRemoved(int id);



    }
}
