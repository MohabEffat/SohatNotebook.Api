using Entities.DbSet;

namespace DataService.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        //Task<User> GetByEmailAddress(string Email);

        Task<bool> UpdateUserProfile(User user);
    }
}
