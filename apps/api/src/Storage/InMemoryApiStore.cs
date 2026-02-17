using TalentSkillHarvester.Api.Contracts;

namespace TalentSkillHarvester.Api.Storage;

public sealed class InMemoryApiStore : IApiStore
{
    private readonly List<SkillItem> skills =
    [
        new SkillItem(1, "C#", "Backend", true, DateTime.UtcNow),
        new SkillItem(2, ".NET", "Backend", true, DateTime.UtcNow),
        new SkillItem(3, "React", "Frontend", true, DateTime.UtcNow)
    ];

    private readonly List<ExtractionLogItem> extractionLogs = [];

    private int nextSkillId = 4;
    private int nextExtractionId = 1;

    public Task<IReadOnlyList<SkillItem>> GetSkillsAsync(CancellationToken cancellationToken = default)
    {
        var result = skills
            .OrderBy(skill => skill.Name)
            .ToList();

        return Task.FromResult<IReadOnlyList<SkillItem>>(result);
    }

    public Task<bool> IsSkillNameTakenAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var result = skills.Any(skill =>
            (excludeId is null || skill.Id != excludeId.Value) &&
            string.Equals(skill.Name, name, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(result);
    }

    public Task<SkillItem> AddSkillAsync(CreateSkillRequest request, CancellationToken cancellationToken = default)
    {
        var newSkill = new SkillItem(
            nextSkillId++,
            request.Name.Trim(),
            request.Category.Trim(),
            request.IsActive ?? true,
            DateTime.UtcNow);

        skills.Add(newSkill);

        return Task.FromResult(newSkill);
    }

    public Task<SkillItem?> UpdateSkillAsync(int id, UpdateSkillRequest request, CancellationToken cancellationToken = default)
    {
        var existingSkill = skills.FirstOrDefault(skill => skill.Id == id);

        if (existingSkill is null)
        {
            return Task.FromResult<SkillItem?>(null);
        }

        var updatedSkill = existingSkill with
        {
            Name = request.Name?.Trim() ?? existingSkill.Name,
            Category = request.Category?.Trim() ?? existingSkill.Category,
            IsActive = request.IsActive ?? existingSkill.IsActive,
            UpdatedAtUtc = DateTime.UtcNow
        };

        var index = skills.FindIndex(skill => skill.Id == id);
        skills[index] = updatedSkill;

        return Task.FromResult<SkillItem?>(updatedSkill);
    }

    public Task AddExtractionLogAsync(CreateExtractionLogEntry entry, CancellationToken cancellationToken = default)
    {
        extractionLogs.Add(new ExtractionLogItem(
            nextExtractionId++,
            DateTime.UtcNow,
            entry.SkillCount,
            entry.WarningCount,
            entry.Summary));

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ExtractionLogItem>> GetExtractionLogsAsync(CancellationToken cancellationToken = default)
    {
        var result = extractionLogs
            .OrderByDescending(log => log.CreatedAtUtc)
            .ToList();

        return Task.FromResult<IReadOnlyList<ExtractionLogItem>>(result);
    }
}