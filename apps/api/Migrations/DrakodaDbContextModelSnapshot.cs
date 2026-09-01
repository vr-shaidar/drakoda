using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

[DbContext(typeof(DrakodaDbContext))]
partial class DrakodaDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.20").HasAnnotation("Relational:MaxIdentifierLength", 63);
        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);
        modelBuilder.Entity("SystemSetting", b => { b.Property<string>("Key").HasMaxLength(128).HasColumnType("character varying(128)"); b.Property<DateTimeOffset>("UpdatedAt").HasColumnType("timestamp with time zone"); b.Property<string>("Value").IsRequired().HasColumnType("jsonb"); b.HasKey("Key"); b.ToTable("system_settings"); });
        modelBuilder.Entity("Drakoda.AI.AIProvider", b => { b.Property<Guid>("Id"); b.Property<string>("Key").IsRequired().HasMaxLength(64).HasColumnType("character varying(64)"); b.Property<string>("DisplayName").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)"); b.Property<bool>("Enabled"); b.HasKey("Id"); b.HasIndex("Key").IsUnique(); b.ToTable("ai_providers"); });
        modelBuilder.Entity("Drakoda.AI.AIModel", b => { b.Property<Guid>("Id"); b.Property<Guid>("ProviderId"); b.Property<string>("ExternalModelId").IsRequired().HasMaxLength(256).HasColumnType("character varying(256)"); b.Property<string>("DisplayName").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)"); b.Property<int>("MediaType"); b.Property<bool>("Enabled"); b.Property<int>("Priority"); b.Property<int>("MaxConcurrency"); b.Property<string>("Capabilities").IsRequired().HasColumnType("jsonb"); b.Property<string>("Metadata").IsRequired().HasColumnType("jsonb"); b.HasKey("Id"); b.HasIndex("ProviderId", "ExternalModelId").IsUnique(); b.HasOne("Drakoda.AI.AIProvider", "Provider").WithMany().HasForeignKey("ProviderId").OnDelete(DeleteBehavior.Restrict).IsRequired(); b.ToTable("ai_models"); });
        modelBuilder.Entity("Drakoda.AI.Generation", b => { b.Property<Guid>("Id"); b.Property<Guid>("ModelId"); b.Property<Guid?>("UserId"); b.Property<int>("Mode"); b.Property<int>("Status"); b.Property<string>("Prompt").IsRequired().HasMaxLength(10000).HasColumnType("character varying(10000)"); b.Property<string>("Settings").IsRequired().HasColumnType("jsonb"); b.Property<string>("SourceAssetIds").IsRequired().HasColumnType("jsonb"); b.Property<string>("IdempotencyKey").HasMaxLength(255).HasColumnType("character varying(255)"); b.Property<string>("ExternalJobId"); b.Property<string>("ProviderRequestId"); b.Property<string>("ErrorCode").HasMaxLength(128).HasColumnType("character varying(128)"); b.Property<string>("ErrorMessage").HasMaxLength(4000).HasColumnType("character varying(4000)"); b.Property<int>("AttemptCount"); b.Property<DateTimeOffset>("CreatedAt").HasColumnType("timestamp with time zone"); b.Property<DateTimeOffset>("UpdatedAt").HasColumnType("timestamp with time zone"); b.Property<DateTimeOffset?>("CompletedAt").HasColumnType("timestamp with time zone"); b.HasKey("Id"); b.HasIndex("IdempotencyKey").IsUnique().HasFilter("\"idempotency_key\" IS NOT NULL"); b.HasIndex("Status", "CreatedAt"); b.ToTable("generations"); });
    }
}
