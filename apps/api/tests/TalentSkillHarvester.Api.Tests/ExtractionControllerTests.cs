using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Controllers;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Tests;

public sealed class ExtractionControllerTests
{
    [Fact]
    public void Extract_ReturnsBadRequest_WhenInputIsMissing()
    {
        var store = new InMemoryApiStore();
        var controller = new ExtractionController(store);

        var result = controller.Extract(new ExtractRequest("", " "));

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void Extract_ReturnsOk_AndWritesExtractionLog()
    {
        var store = new InMemoryApiStore();
        var controller = new ExtractionController(store);
        var cv = "Experienced C# and .NET engineer with Azure and SQL background.";
        var ifu = "Role requires React and TypeScript skills with CI/CD knowledge.";

        var result = controller.Extract(new ExtractRequest(cv, ifu));

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<ExtractResponse>(ok.Value);

        Assert.NotEmpty(payload.Skills);
        Assert.Contains(payload.Skills, skill => skill.Name == "C#");

        var logs = store.GetExtractionLogs();
        Assert.Single(logs);
        Assert.Equal(payload.Skills.Count, logs[0].SkillCount);
        Assert.Equal(payload.Warnings.Count, logs[0].WarningCount);
    }
}
