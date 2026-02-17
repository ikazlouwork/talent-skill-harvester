using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Controllers;

[ApiController]
[Route("api/skills")]
public sealed class SkillsController(InMemoryApiStore store) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SkillItem>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<SkillItem>> GetSkills()
    {
        return Ok(store.GetSkills());
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkillItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<SkillItem> CreateSkill([FromBody] CreateSkillRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Category))
        {
            return BadRequest(new { error = "Name and Category are required." });
        }

        if (store.IsSkillNameTaken(request.Name))
        {
            return Conflict(new { error = "Skill with this name already exists." });
        }

        var createdSkill = store.AddSkill(request);

        return Created($"/api/skills/{createdSkill.Id}", createdSkill);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(SkillItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<SkillItem> PatchSkill(int id, [FromBody] UpdateSkillRequest request)
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

            if (store.IsSkillNameTaken(request.Name, id))
            {
                return Conflict(new { error = "Skill with this name already exists." });
            }
        }

        if (request.Category is not null && string.IsNullOrWhiteSpace(request.Category))
        {
            return BadRequest(new { error = "Category cannot be empty." });
        }

        var updatedSkill = store.UpdateSkill(id, request);

        if (updatedSkill is null)
        {
            return NotFound(new { error = "Skill not found." });
        }

        return Ok(updatedSkill);
    }
}