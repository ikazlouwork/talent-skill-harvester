using TalentSkillHarvester.Api.Contracts;
using TalentSkillHarvester.Api.Persistence.Entities;
using TalentSkillHarvester.Api.Persistence.Repositories;

namespace TalentSkillHarvester.Api.Storage;

public sealed class ApiStoreService(ISkillRepository skillRepository, IExtractionLogRepository extractionLogRepository) : IApiStore
{
    public async Task<IReadOnlyList<SkillItem>> GetSkillsAsync(CancellationToken cancellationToken = default)
    {
        var skills = await skillRepository.GetSkillsAsync(cancellationToken);

        return skills
            .Select(skill => new SkillItem(skill.Id, skill.Name, skill.Category, skill.IsActive, skill.UpdatedAtUtc))
            .ToList();
    }

    public Task<bool> IsSkillNameTakenAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return skillRepository.IsSkillNameTakenAsync(name.Trim(), excludeId, cancellationToken);
    }

    public async Task<SkillItem> AddSkillAsync(CreateSkillRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new SkillEntity
        {
            Name = request.Name.Trim(),
            Category = request.Category.Trim(),
            IsActive = request.IsActive ?? true,
            UpdatedAtUtc = DateTime.UtcNow
        };

        var createdSkill = await skillRepository.AddSkillAsync(entity, cancellationToken);

        return new SkillItem(
            createdSkill.Id,
            createdSkill.Name,
            createdSkill.Category,
            createdSkill.IsActive,
            createdSkill.UpdatedAtUtc);
    }

    public async Task<SkillItem?> UpdateSkillAsync(int id, UpdateSkillRequest request, CancellationToken cancellationToken = default)
    {
        var existingSkill = await skillRepository.GetByIdAsync(id, cancellationToken);

        if (existingSkill is null)
        {
            return null;
        }

        existingSkill.Name = request.Name?.Trim() ?? existingSkill.Name;
        existingSkill.Category = request.Category?.Trim() ?? existingSkill.Category;
        existingSkill.IsActive = request.IsActive ?? existingSkill.IsActive;
        existingSkill.UpdatedAtUtc = DateTime.UtcNow;

        var updatedSkill = await skillRepository.UpdateSkillAsync(existingSkill, cancellationToken);

        return new SkillItem(
            updatedSkill.Id,
            updatedSkill.Name,
            updatedSkill.Category,
            updatedSkill.IsActive,
            updatedSkill.UpdatedAtUtc);
    }

    public async Task AddExtractionLogAsync(CreateExtractionLogEntry entry, CancellationToken cancellationToken = default)
    {
        var log = new ExtractionLogEntity
        {
            CreatedAtUtc = DateTime.UtcNow,
            SkillCount = entry.SkillCount,
            WarningCount = entry.WarningCount,
            Summary = entry.Summary
        };

        await extractionLogRepository.AddExtractionLogAsync(log, cancellationToken);
    }

    public async Task<IReadOnlyList<ExtractionLogItem>> GetExtractionLogsAsync(CancellationToken cancellationToken = default)
    {
        var logs = await extractionLogRepository.GetExtractionLogsAsync(cancellationToken);

        return logs
            .Select(log => new ExtractionLogItem(log.Id, log.CreatedAtUtc, log.SkillCount, log.WarningCount, log.Summary))
            .ToList();
    }
}