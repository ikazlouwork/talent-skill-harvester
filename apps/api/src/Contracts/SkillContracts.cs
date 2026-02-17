namespace TalentSkillHarvester.Api.Contracts;

public sealed record SkillItem(
    int Id,
    string Name,
    string Category,
    bool IsActive,
    DateTime UpdatedAtUtc);

public sealed record CreateSkillRequest(
    string Name,
    string Category,
    bool? IsActive);

public sealed record UpdateSkillRequest(
    string? Name,
    string? Category,
    bool? IsActive);