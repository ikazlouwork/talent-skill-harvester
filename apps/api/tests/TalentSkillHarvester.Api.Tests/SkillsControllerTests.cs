using Microsoft.AspNetCore.Mvc;
using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Controllers;
using TalentSkillHarvester.Api.Storage;

namespace TalentSkillHarvester.Api.Tests;

public sealed class SkillsControllerTests
{
    [Fact]
    public void CreateSkill_ReturnsCreated_ForValidRequest()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = controller.CreateSkill(new CreateSkillRequest("Kubernetes", "DevOps", true));

        var created = Assert.IsType<CreatedResult>(result.Result);
        var skill = Assert.IsType<SkillItem>(created.Value);

        Assert.Equal("Kubernetes", skill.Name);
        Assert.Equal("DevOps", skill.Category);
    }

    [Fact]
    public void CreateSkill_ReturnsConflict_ForDuplicateName()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = controller.CreateSkill(new CreateSkillRequest("c#", "Backend", true));

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public void PatchSkill_ReturnsOk_WhenSkillExists()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = controller.PatchSkill(1, new UpdateSkillRequest("CSharp", "Backend", false));

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var skill = Assert.IsType<SkillItem>(ok.Value);

        Assert.Equal("CSharp", skill.Name);
        Assert.False(skill.IsActive);
    }

    [Fact]
    public void PatchSkill_ReturnsNotFound_WhenSkillDoesNotExist()
    {
        var store = new InMemoryApiStore();
        var controller = new SkillsController(store);

        var result = controller.PatchSkill(999, new UpdateSkillRequest("Any", null, null));

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
