using Microsoft.EntityFrameworkCore;
using Drakoda.AI;
using Drakoda.Api.Domain.Generations;

public sealed class DrakodaDbContext(DbContextOptions<DrakodaDbContext> options) : DbContext(options)
{
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<AIProvider> AIProviders => Set<AIProvider>();
    public DbSet<AIModel> AIModels => Set<AIModel>();
    public DbSet<Generation> Generations => Set<Generation>();
    public DbSet<GenerationJob> GenerationJobs => Set<GenerationJob>();
    public DbSet<GenerationOutput> GenerationOutputs => Set<GenerationOutput>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.ToTable("system_settings"); entity.HasKey(x => x.Key); entity.Property(x => x.Key).HasMaxLength(128); entity.Property(x => x.Value).HasColumnType("jsonb");
        });
        modelBuilder.Entity<AIProvider>(entity =>
        {
            entity.ToTable("ai_providers"); entity.HasKey(x => x.Id); entity.HasIndex(x => x.Key).IsUnique(); entity.Property(x => x.Key).HasMaxLength(64); entity.Property(x => x.DisplayName).HasMaxLength(128);
        });
        modelBuilder.Entity<AIModel>(entity =>
        {
            entity.ToTable("ai_models"); entity.HasKey(x => x.Id); entity.HasIndex(x => new { x.ProviderId, x.ExternalModelId }).IsUnique(); entity.Property(x => x.ExternalModelId).HasMaxLength(256); entity.Property(x => x.DisplayName).HasMaxLength(128); entity.Property(x => x.Capabilities).HasColumnType("jsonb"); entity.Property(x => x.Metadata).HasColumnType("jsonb");
            entity.HasOne(x => x.Provider).WithMany().HasForeignKey(x => x.ProviderId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Generation>(entity =>
        {
            entity.ToTable("generations"); entity.HasKey(x => x.Id); entity.HasIndex(x => x.IdempotencyKey).IsUnique().HasFilter("\"idempotency_key\" IS NOT NULL"); entity.HasIndex(x => new { x.Status, x.CreatedAt });
            entity.Property(x => x.Prompt).HasMaxLength(10000); entity.Property(x => x.Settings).HasColumnType("jsonb"); entity.Property(x => x.SourceAssetIds).HasColumnType("jsonb"); entity.Property(x => x.IdempotencyKey).HasMaxLength(255); entity.Property(x => x.ErrorCode).HasMaxLength(128); entity.Property(x => x.ErrorMessage).HasMaxLength(4000);
        });
        GenerationPersistence.Configure(modelBuilder);
    }
}

public sealed class SystemSetting
{
    public required string Key { get; set; }
    public required string Value { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
