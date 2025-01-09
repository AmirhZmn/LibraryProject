using Microsoft.EntityFrameworkCore;
using ModularPatternTraining.Data.AppDbContext;

namespace ModularPatternTraining.Shared.DataAccess
{
    public class DataRepository<T>:IDataRepository<T> where T : class
    {
        protected AppDbContext _appDbContext;

        public DataRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {

            return await _appDbContext.Set<T>()
                .Where(e=>EF.Property<bool>(e, "IsRemoved") == false)
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _appDbContext.Set<T>()
                .Where(e => EF.Property<bool>(e, "IsRemoved") == false)
                .ToListAsync()
                .ConfigureAwait(false);

        }

        public async Task AddAsync(T entity)
        {
           await _appDbContext.Set<T>().AddAsync(entity).ConfigureAwait(false);
        }

       

        public async Task SaveChangesAsync()
        {
            
            await _appDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task SafeSave(Func<Task> dbOperations)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                await dbOperations();
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch
            {
               
                await transaction.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }

        public async Task<bool> IsExist(string name)
        {
            
            return await _appDbContext.Set<T>().AnyAsync(e => EF.Property<string>(e, "Name") == name).ConfigureAwait(false);
        }

        public async Task<bool> IsExistById(int id)
        {
            return await _appDbContext.Set<T>().AnyAsync(e => EF.Property<int>(e, "Id") == id).ConfigureAwait(false);
        }

        public async Task<bool> IsRemoved(int id)
        {
          
            return await _appDbContext.Set<T>()
                .AnyAsync(e =>EF.Property<int>(e, "Id") == id && EF.Property<bool>(e, "IsRemoved") == true).ConfigureAwait(false);
        }

       
    }
}
