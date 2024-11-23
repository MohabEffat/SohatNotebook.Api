using DataService.Data;
using DataService.IRepository;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;

namespace DataService.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(
            AppDbContext context
            )
        {
            _context = context;
            dbSet = _context.Set<TEntity>();
        }
        public Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid Id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
