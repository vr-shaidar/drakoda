using Microsoft.EntityFrameworkCore;
using Drakoda.AI;
using Drakoda.Api.Domain.Generations;
using Drakoda.Api.Domain.Assets;
using Drakoda.Api.Domain.Billing;
using Drakoda.Api.Domain.Pricing;

public sealed class DrakodaDbContext(DbContextOptions<DrakodaDbContext> options) : DbContext(options)
{
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<AIProvider> AIProviders => Set<AIProvider>();
    public DbSet<AIModel> AIModels => Set<AIModel>();
    public DbSet<Generation> Generations => Set<Generation>();
    public DbSet<GenerationJob> GenerationJobs => Set<GenerationJob>();
    public DbSet<GenerationOutput> GenerationOutputs => Set<GenerationOutput>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<CreditWallet> CreditWallets => Set<CreditWallet>();
    public DbSet<CreditTransaction> CreditTransactions => Set<CreditTransaction>();
    public DbSet<ModelPricing> ModelPricing => Set<ModelPricing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemSetting>(entity => { entity.ToTable("system_settings"); entity.HasKey(x => x.Key); entity.Property(x => x.Key).HasMaxLength(128); entity.Property(x => x.Value).HasColumnType("jsonb"); });
        modelBuilder.Entity<AIProvider>(entity => { entity.ToTable("ai_providers"); entity.HasKey(x => x.Id); entity.HasIndex(x => x.Key).IsUnique(); entity.Property(x => x.Key).HasMaxLength(64); entity.Property(x => x.DisplayName).HasMaxLength(128); });
        modelBuilder.Entity<AIModel>(entity => { entity.ToTable("ai_models"); entity.HasKey(x => x.Id); entity.HasIndex(x => new { x.ProviderId, x.ExternalModelId }).IsUnique(); entity.Property(x => x.ExternalModelId).HasMaxLength(256); entity.Property(x => x.DisplayName).HasMaxLength(128); entity.Property(x => x.Capabilities).HasColumnType("jsonb"); entity.Property(x => x.Metadata).HasColumnType("jsonb"); entity.HasOne(x => x.Provider).WithMany().HasForeignKey(x => x.ProviderId).OnDelete(DeleteBehavior.Restrict); });
        modelBuilder.Entity<Generation>(entity => { entity.ToTable("generations"); entity.HasKey(x => x.Id); entity.HasIndex(x => x.IdempotencyKey).IsUnique().HasFilter("\"idempotency_key\" IS NOT NULL"); entity.HasIndex(x => new { x.Status, x.CreatedAt }); entity.Property(x => x.Prompt).HasMaxLength(10000); entity.Property(x => x.Settings).HasColumnType("jsonb"); entity.Property(x => x.SourceAssetIds).HasColumnType("jsonb"); entity.Property(x => x.IdempotencyKey).HasMaxLength(255); entity.Property(x => x.ErrorCode).HasMaxLength(128); entity.Property(x => x.ErrorMessage).HasMaxLength(4000); });
        GenerationPersistence.Configure(modelBuilder);
        modelBuilder.Entity<Project>(entity => { entity.ToTable("projects"); entity.HasKey(x => x.Id); entity.HasIndex(x => new { x.UserId, x.CreatedAt }); entity.Property(x => x.Name).HasMaxLength(200); });
        modelBuilder.Entity<Asset>(entity => { entity.ToTable("assets"); entity.HasKey(x => x.Id); entity.HasIndex(x => new { x.UserId, x.CreatedAt }); entity.HasIndex(x => x.ProjectId); entity.Property(x => x.FileName).HasMaxLength(512); entity.Property(x => x.StorageKey).HasMaxLength(1024); entity.Property(x => x.ContentType).HasMaxLength(128); entity.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.SetNull); });
        modelBuilder.Entity<CreditWallet>(entity => { entity.ToTable("credit_wallets"); entity.HasKey(x => x.Id); entity.HasIndex(x => x.UserId).IsUnique(); entity.Property(x => x.AvailableCredits).HasPrecision(20, 6); entity.Property(x => x.ReservedCredits).HasPrecision(20, 6); });
        modelBuilder.Entity<CreditTransaction>(entity => { entity.ToTable("credit_transactions"); entity.HasKey(x => x.Id); entity.HasIndex(x => new { x.WalletId, x.IdempotencyKey }).IsUnique(); entity.HasIndex(x => new { x.WalletId, x.CreatedAt }); entity.Property(x => x.Amount).HasPrecision(20, 6); entity.Property(x => x.BalanceAfter).HasPrecision(20, 6); entity.Property(x => x.IdempotencyKey).HasMaxLength(255); entity.Property(x => x.Description).HasMaxLength(1000); });
        modelBuilder.Entity<ModelPricing>(entity => { entity.ToTable("model_pricing"); entity.HasKey(x => x.Id); entity.HasIndex(x => new { x.ModelId, x.Version, x.Unit }).IsUnique(); entity.HasIndex(x => new { x.ModelId, x.EffectiveFrom }); entity.Property(x => x.ProviderCostPerUnit).HasPrecision(20, 8); entity.Property(x => x.CustomerPricePerUnit).HasPrecision(20, 8); entity.Property(x => x.MinimumCharge).HasPrecision(20, 8); entity.Property(x => x.Multiplier).HasPrecision(20, 8); entity.HasOne<AIModel>().WithMany().HasForeignKey(x => x.ModelId).OnDelete(DeleteBehavior.Cascade); });
    }
}

public sealed class SystemSetting { public required string Key { get; set; } public required string Value { get; set; } public DateTimeOffset UpdatedAt { get; set; } }
