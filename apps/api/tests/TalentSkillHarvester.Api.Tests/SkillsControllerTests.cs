using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Controllers;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Tests;

public sealed class SkillsControllerTests
{
    [Fact]
    public async Task CreateSkill_ReturnsCreated_ForValidRequest()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = await controller.CreateSkill(new CreateSkillRequest("Kubernetes", "DevOps", true), CancellationToken.None);

        var created = Assert.IsType<CreatedResult>(result.Result);
        var skill = Assert.IsType<SkillItem>(created.Value);

        Assert.Equal("Kubernetes", skill.Name);
        Assert.Equal("DevOps", skill.Category);
    }

    [Fact]
    public async Task CreateSkill_ReturnsConflict_ForDuplicateName()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = await controller.CreateSkill(new CreateSkillRequest("c#", "Backend", true), CancellationToken.None);

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task PatchSkill_ReturnsOk_WhenSkillExists()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = await controller.PatchSkill(1, new UpdateSkillRequest("CSharp", "Backend", false), CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var skill = Assert.IsType<SkillItem>(ok.Value);

        Assert.Equal("CSharp", skill.Name);
        Assert.False(skill.IsActive);
    }

    [Fact]
    public async Task PatchSkill_ReturnsNotFound_WhenSkillDoesNotExist()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = await controller.PatchSkill(999, new UpdateSkillRequest("Any", null, null), CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
