using Microsoft.EntityFrameworkCore;
using TalentSkillHarvester.Api.Persistence.Entities;

namespace TalentSkillHarvester.Api.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var hasAnySkills = await dbContext.Skills.AnyAsync(cancellationToken);

        if (hasAnySkills)
        {
            return;
        }

        var now = DateTime.UtcNow;

        dbContext.Skills.AddRange(
            new SkillEntity { Name = "C#", Category = "Backend", IsActive = true, UpdatedAtUtc = now },
            new SkillEntity { Name = ".NET", Category = "Backend", IsActive = true, UpdatedAtUtc = now },
            new SkillEntity { Name = "React", Category = "Frontend", IsActive = true, UpdatedAtUtc = now });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}