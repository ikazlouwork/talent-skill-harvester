using Microsoft.EntityFrameworkCore;
using TalentSkillHarvester.Api.Persistence.Entities;

namespace TalentSkillHarvester.Api.Persistence.Repositories;

public sealed class SqliteSkillRepository(AppDbContext dbContext) : ISkillRepository
{
    public async Task<IReadOnlyList<SkillEntity>> GetSkillsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Skills
            .AsNoTracking()
            .OrderBy(skill => skill.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsSkillNameTakenAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.ToLower();

        return await dbContext.Skills.AnyAsync(
            skill =>
                (!excludeId.HasValue || skill.Id != excludeId.Value) &&
                skill.Name.ToLower() == normalizedName,
            cancellationToken);
    }

    public async Task<SkillEntity> AddSkillAsync(SkillEntity skill, CancellationToken cancellationToken = default)
    {
        dbContext.Skills.Add(skill);
        await dbContext.SaveChangesAsync(cancellationToken);

        return skill;
    }

    public async Task<SkillEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Skills.FirstOrDefaultAsync(skill => skill.Id == id, cancellationToken);
    }

    public async Task<SkillEntity> UpdateSkillAsync(SkillEntity skill, CancellationToken cancellationToken = default)
    {
        dbContext.Skills.Update(skill);
        await dbContext.SaveChangesAsync(cancellationToken);

        return skill;
    }
}