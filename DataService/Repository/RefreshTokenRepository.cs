using DataService.Data;
using DataService.IRepository;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataService.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<RefreshToken>> GetAllAsync()
        {
            try
            {
                return await dbSet.Where(x => x.Status == 1)
                            .AsNoTracking()
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RefreshTokenRepository)} all Method Has Generated An Error");
                return Enumerable.Empty<RefreshToken>();
            }
        }

        public async Task<RefreshToken?> GetByRefreshToken(string refreshToken)
        {
            try
            {
                return await dbSet.Where(x => x.Token.ToLower() == refreshToken.ToLower())
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RefreshTokenRepository)} GetByRefreshToken Method Has Generated An Error");
                return null;
            }
        }

        public async Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken)
        {
            try
            {
                var token =  await dbSet.Where(x => x.Token.ToLower() == refreshToken.Token.ToLower())
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
                if (token == null) return false;
                token.IsUsed = refreshToken.IsUsed;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(RefreshTokenRepository)} MarkRefreshTokenAsUsed Method Has Generated An Error");
                return false;
            }
        }
    }
}
