using DataService.IConfiguration;
using DataService.IRepository;
using DataService.Repository;
using Microsoft.Extensions.Logging;

namespace DataService.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("db_logs");

            Users = new UserRepository(context, _logger);
            RefreshTokens = new RefreshTokenRepository(context, _logger);
        }

        public IUserRepository Users { get; private set; }
        public IRefreshTokenRepository RefreshTokens { get; private set; }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
