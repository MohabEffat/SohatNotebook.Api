using DataService.IRepository;

namespace DataService.IConfiguration
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        Task CompleteAsync();
    }
}
