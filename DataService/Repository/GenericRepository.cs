using DataService.Data;
using DataService.IRepository;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataService.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _context;
        protected readonly ILogger _logger;
        internal DbSet<TEntity> dbSet;


        public GenericRepository(
            AppDbContext context,
            ILogger logger
            )
        {
            _context = context;
            _logger = logger;
            dbSet = _context.Set<TEntity>();
        }
        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual Task<bool> Delete(Guid Id, string userId)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid? Id)
        {
            return await dbSet.FindAsync(Id);
        }

        public Task<TEntity> UpsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
