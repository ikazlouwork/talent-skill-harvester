using TalentSkillHarvester.Api.Persistence.Entities;

namespace TalentSkillHarvester.Api.Persistence.Repositories;

public interface IExtractionLogRepository
{
    Task<IReadOnlyList<ExtractionLogEntity>> GetExtractionLogsAsync(CancellationToken cancellationToken = default);

    Task<ExtractionLogEntity> AddExtractionLogAsync(ExtractionLogEntity log, CancellationToken cancellationToken = default);
}