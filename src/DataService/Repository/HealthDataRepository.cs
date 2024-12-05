using DataService.Data;
using DataService.IRepository;
using Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataService.Repository
{
    public class HealthDataRepository : GenericRepository<HealthData>, IHealthDataRepository
    {
        public HealthDataRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }
        public override async Task<IEnumerable<HealthData>> GetAllAsync()
        {
            try
            {
                return await dbSet.Where(x => x.Status == 1)
                            .AsNoTracking()
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(HealthDataRepository)} all Method Has Generated An Error");
                return Enumerable.Empty<HealthData>();
            }
        }
        public async Task<bool> UpdateHealthDataAsync(HealthData healthData)
        {
            try
            {
                var existinghealthData = await dbSet.Where(x => x.Status == 1 && x.Id == healthData.Id)
                            .FirstOrDefaultAsync();
                if (existinghealthData is null) return false;

                existinghealthData.BloodType = healthData.BloodType;
                existinghealthData.Height = healthData.Height;
                existinghealthData.Weoight = healthData.Weoight;
                existinghealthData.Race = healthData.Race;
                existinghealthData.UseGlasses = healthData.UseGlasses;
                existinghealthData.UpdateDate = healthData.UpdateDate;

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{typeof(HealthDataRepository)} all Method Has Generated An Error");
                return false;
            }
        }
    }
}
