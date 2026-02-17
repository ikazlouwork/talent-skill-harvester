using TalentSkillHarvester.Api.Contracts;

namespace TalentSkillHarvester.Api.Storage;

public sealed class InMemoryApiStore
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

    public IReadOnlyList<SkillItem> GetSkills()
    {
        return skills
            .OrderBy(skill => skill.Name)
            .ToList();
    }

    public bool IsSkillNameTaken(string name, int? excludeId = null)
    {
        return skills.Any(skill =>
            (excludeId is null || skill.Id != excludeId.Value) &&
            string.Equals(skill.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    public SkillItem AddSkill(CreateSkillRequest request)
    {
        var newSkill = new SkillItem(
            nextSkillId++,
            request.Name.Trim(),
            request.Category.Trim(),
            request.IsActive ?? true,
            DateTime.UtcNow);

        skills.Add(newSkill);

        return newSkill;
    }

    public SkillItem? UpdateSkill(int id, UpdateSkillRequest request)
    {
        var existingSkill = skills.FirstOrDefault(skill => skill.Id == id);

        if (existingSkill is null)
        {
            return null;
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

        return updatedSkill;
    }

    public void AddExtractionLog(CreateExtractionLogEntry entry)
    {
        extractionLogs.Add(new ExtractionLogItem(
            nextExtractionId++,
            DateTime.UtcNow,
            entry.SkillCount,
            entry.WarningCount,
            entry.Summary));
    }

    public IReadOnlyList<ExtractionLogItem> GetExtractionLogs()
    {
        return extractionLogs
            .OrderByDescending(log => log.CreatedAtUtc)
            .ToList();
    }
}