using TalentSkillHarvester.Api.Persistence.Entities;

namespace TalentSkillHarvester.Api.Persistence.Repositories;

public interface ISkillRepository
{
    Task<IReadOnlyList<SkillEntity>> GetSkillsAsync(CancellationToken cancellationToken = default);

    Task<bool> IsSkillNameTakenAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<SkillEntity> AddSkillAsync(SkillEntity skill, CancellationToken cancellationToken = default);

    Task<SkillEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<SkillEntity> UpdateSkillAsync(SkillEntity skill, CancellationToken cancellationToken = default);
}