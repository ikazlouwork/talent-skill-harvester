namespace TalentSkillHarvester.Api.Contracts;

public sealed record ExtractRequest(string CvText, string IfuText);

public sealed record ExtractResponse(
    string Summary,
    IReadOnlyList<ExtractedSkill> Skills,
    IReadOnlyList<string> Warnings);

public sealed record ExtractedSkill(
    string Name,
    string Category,
    string Evidence,
    int Confidence);
