using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Controllers;

[ApiController]
[Route("api/skills")]
public sealed class SkillsController(IApiStore store) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SkillItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SkillItem>>> GetSkills(CancellationToken cancellationToken)
    {
        return Ok(await store.GetSkillsAsync(cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkillItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SkillItem>> CreateSkill([FromBody] CreateSkillRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Category))
        {
            return BadRequest(new { error = "Name and Category are required." });
        }

        if (await store.IsSkillNameTakenAsync(request.Name, cancellationToken: cancellationToken))
        {
            return Conflict(new { error = "Skill with this name already exists." });
        }

        var createdSkill = await store.AddSkillAsync(request, cancellationToken);

        return Created($"/api/skills/{createdSkill.Id}", createdSkill);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(SkillItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SkillItem>> PatchSkill(int id, [FromBody] UpdateSkillRequest request, CancellationToken cancellationToken)
    {
        var noChangesProvided = request.Name is null && request.Category is null && request.IsActive is null;

        if (noChangesProvided)
        {
            return BadRequest(new { error = "At least one property must be provided for update." });
        }

        if (request.Name is not null)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { error = "Name cannot be empty." });
            }

            if (await store.IsSkillNameTakenAsync(request.Name, id, cancellationToken))
            {
                return Conflict(new { error = "Skill with this name already exists." });
            }
        }

        if (request.Category is not null && string.IsNullOrWhiteSpace(request.Category))
        {
            return BadRequest(new { error = "Category cannot be empty." });
        }

        var updatedSkill = await store.UpdateSkillAsync(id, request, cancellationToken);

        if (updatedSkill is null)
        {
            return NotFound(new { error = "Skill not found." });
        }

        return Ok(updatedSkill);
    }
}