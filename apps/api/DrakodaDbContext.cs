using Microsoft.EntityFrameworkCore;
using Drakoda.AI;

public sealed class DrakodaDbContext(DbContextOptions<DrakodaDbContext> options) : DbContext(options)
{
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<AIProvider> AIProviders => Set<AIProvider>();
    public DbSet<AIModel> AIModels => Set<AIModel>();

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
    }
}

public sealed class SystemSetting
{
    public required string Key { get; set; }
    public required string Value { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
