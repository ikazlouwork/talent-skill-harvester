using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Controllers;

[ApiController]
[Route("api/extractions")]
public sealed class ExtractionsController(IApiStore store) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExtractionLogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ExtractionLogItem>>> GetExtractions(CancellationToken cancellationToken)
    {
        return Ok(await store.GetExtractionLogsAsync(cancellationToken));
    }
}