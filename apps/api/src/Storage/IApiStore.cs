using TalentSkillHarvester.Api.Contracts;

namespace TalentSkillHarvester.Api.Storage;

public interface IApiStore
{
    Task<IReadOnlyList<SkillItem>> GetSkillsAsync(CancellationToken cancellationToken = default);

    Task<bool> IsSkillNameTakenAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<SkillItem> AddSkillAsync(CreateSkillRequest request, CancellationToken cancellationToken = default);

    Task<SkillItem?> UpdateSkillAsync(int id, UpdateSkillRequest request, CancellationToken cancellationToken = default);

    Task AddExtractionLogAsync(CreateExtractionLogEntry entry, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExtractionLogItem>> GetExtractionLogsAsync(CancellationToken cancellationToken = default);
}