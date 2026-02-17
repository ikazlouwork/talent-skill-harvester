namespace TalentSkillHarvester.Api.Persistence.Entities;

public sealed class ExtractionLogEntity
{
    public int Id { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public int SkillCount { get; set; }

    public int WarningCount { get; set; }

    public string Summary { get; set; } = string.Empty;
}