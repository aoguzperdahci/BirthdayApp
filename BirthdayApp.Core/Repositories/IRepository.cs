namespace BirthdayApp.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);

        ValueTask<TEntity> GetByIdAsync(long id);

        ValueTask<IEnumerable<TEntity>> GetAllAsync();

        Task UpdateAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);
    }
}