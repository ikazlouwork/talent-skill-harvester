namespace TalentSkillHarvester.Api.Persistence.Entities;

public sealed class SkillEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}