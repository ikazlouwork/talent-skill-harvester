using Microsoft.EntityFrameworkCore;
using TalentSkillHarvester.Api.Persistence.Entities;

namespace TalentSkillHarvester.Api.Persistence.Repositories;

public sealed class SqliteExtractionLogRepository(AppDbContext dbContext) : IExtractionLogRepository
{
    public async Task<IReadOnlyList<ExtractionLogEntity>> GetExtractionLogsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ExtractionLogs
            .AsNoTracking()
            .OrderByDescending(log => log.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<ExtractionLogEntity> AddExtractionLogAsync(ExtractionLogEntity log, CancellationToken cancellationToken = default)
    {
        dbContext.ExtractionLogs.Add(log);
        await dbContext.SaveChangesAsync(cancellationToken);

        return log;
    }
}