using DataService.Data;
using DataService.IRepository;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataService.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await dbSet.Where(x => x.Status == 1)
                            .AsNoTracking()
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(UserRepository)} all Method Has Generated An Error");
                return Enumerable.Empty<User>();
            }
        }

        public async Task<User?> GetByIdentityIdAsync(Guid identityId)
        {
            try
            {
                return await dbSet.Where(x => x.Status == 1 && x.IdentityId == identityId)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(UserRepository)} all Method Has Generated An Error");
                return null;
            }
        }

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            try
            {
                var existingUser =  await dbSet.Where(x => x.Status == 1 && x.Id == user.Id)
                            .FirstOrDefaultAsync();
                if (existingUser is null)
                    return false;
                existingUser.UpdateDate = DateTime.Now;
                existingUser.Sex = user.Sex;
                existingUser.Address = user.Address;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(UserRepository)} all Method Has Generated An Error");
                return false;
            }
        }
    }
}
