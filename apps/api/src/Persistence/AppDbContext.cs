using Microsoft.EntityFrameworkCore;
using TalentSkillHarvester.Api.Persistence.Entities;

namespace TalentSkillHarvester.Api.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SkillEntity> Skills => Set<SkillEntity>();

    public DbSet<ExtractionLogEntity> ExtractionLogs => Set<ExtractionLogEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkillEntity>(entity =>
        {
            entity.ToTable("Skills");
            entity.HasKey(skill => skill.Id);
            entity.Property(skill => skill.Id).ValueGeneratedOnAdd();
            entity.Property(skill => skill.Name).HasMaxLength(128).IsRequired();
            entity.Property(skill => skill.Category).HasMaxLength(64).IsRequired();
            entity.Property(skill => skill.IsActive).IsRequired();
            entity.Property(skill => skill.UpdatedAtUtc).IsRequired();
            entity.HasIndex(skill => skill.Name).IsUnique();
        });

        modelBuilder.Entity<ExtractionLogEntity>(entity =>
        {
            entity.ToTable("ExtractionLogs");
            entity.HasKey(log => log.Id);
            entity.Property(log => log.Id).ValueGeneratedOnAdd();
            entity.Property(log => log.CreatedAtUtc).IsRequired();
            entity.Property(log => log.SkillCount).IsRequired();
            entity.Property(log => log.WarningCount).IsRequired();
            entity.Property(log => log.Summary).HasMaxLength(512).IsRequired();
            entity.HasIndex(log => log.CreatedAtUtc);
        });
    }
}