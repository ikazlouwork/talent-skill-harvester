using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Controllers;

[ApiController]
[Route("api/extractions")]
public sealed class ExtractionsController(InMemoryApiStore store) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExtractionLogItem>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<ExtractionLogItem>> GetExtractions()
    {
        return Ok(store.GetExtractionLogs());
    }
}