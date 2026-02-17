using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class ExtractionController(IApiStore store) : ControllerBase
{
    private static readonly (string Name, string Category)[] KnownSkills =
    [
        ("C#", "Backend"),
        (".NET", "Backend"),
        ("ASP.NET Core", "Backend"),
        ("SQL", "Data"),
        ("Azure", "Cloud"),
        ("React", "Frontend"),
        ("TypeScript", "Frontend"),
        ("JavaScript", "Frontend"),
        ("Docker", "DevOps"),
        ("Git", "Tooling"),
        ("CI/CD", "DevOps")
    ];

    [HttpPost("extract")]
    [ProducesResponseType(typeof(ExtractResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExtractResponse>> Extract([FromBody] ExtractRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CvText) || string.IsNullOrWhiteSpace(request.IfuText))
        {
            return BadRequest(new { error = "Both CvText and IfuText are required." });
        }

        var combinedText = $"{request.CvText}\n{request.IfuText}";

        var extractedSkills = KnownSkills
            .Where(skill => combinedText.Contains(skill.Name, StringComparison.OrdinalIgnoreCase))
            .Select(skill => new ExtractedSkill(
                skill.Name,
                skill.Category,
                $"Detected from candidate text mentioning '{skill.Name}'.",
                80))
            .DistinctBy(skill => skill.Name)
            .ToList();

        if (extractedSkills.Count == 0)
        {
            extractedSkills.Add(new ExtractedSkill(
                "Communication",
                "Soft skill",
                "Fallback skill when no explicit technical keyword is found.",
                55));
        }

        var warnings = new List<string>();

        if (request.CvText.Length < 80)
        {
            warnings.Add("CV input is short; extraction confidence may be lower.");
        }

        if (request.IfuText.Length < 80)
        {
            warnings.Add("IFU input is short; add more context for better extraction quality.");
        }

        var response = new ExtractResponse(
            Summary: $"Extracted {extractedSkills.Count} skill(s) from the provided CV and IFU.",
            Skills: extractedSkills,
            Warnings: warnings);

        await store.AddExtractionLogAsync(new CreateExtractionLogEntry(
            response.Summary,
            response.Skills.Count,
            response.Warnings.Count), cancellationToken);

        return Ok(response);
    }
}
