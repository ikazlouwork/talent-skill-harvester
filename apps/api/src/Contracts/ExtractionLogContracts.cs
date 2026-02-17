namespace TalentSkillHarvester.Api.Contracts;

public sealed record ExtractionLogItem(
    int Id,
    DateTime CreatedAtUtc,
    int SkillCount,
    int WarningCount,
    string Summary);

public sealed record CreateExtractionLogEntry(
    string Summary,
    int SkillCount,
    int WarningCount);