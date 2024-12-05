using Entities.DbSet;

namespace DataService.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid? Id);
        Task<bool> AddAsync(TEntity entity);
        Task<TEntity> UpsertAsync(TEntity entity);
        Task<bool> Delete(Guid Id, string userId);


    }
}
