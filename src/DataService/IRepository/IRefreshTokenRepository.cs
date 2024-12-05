using Entities.DbSet;

namespace DataService.IRepository
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByRefreshToken(string refreshToken);

        Task<bool> MarkRefreshTokenAsUsed (RefreshToken refreshToken);
    }
}
