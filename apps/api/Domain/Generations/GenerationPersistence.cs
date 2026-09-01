using Microsoft.EntityFrameworkCore;

namespace Drakoda.Api.Domain.Generations;

public static class GenerationPersistence
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GenerationJob>(entity =>
        {
            entity.ToTable("generation_jobs");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.GenerationId, x.Attempt }).IsUnique();
            entity.HasIndex(x => new { x.NextAttemptAt, x.LeaseUntil });
            entity.Property(x => x.QueueMessageId).HasMaxLength(128);
            entity.Property(x => x.ProviderJobId).HasMaxLength(512);
            entity.Property(x => x.LastErrorCode).HasMaxLength(128);
            entity.Property(x => x.LastErrorMessage).HasMaxLength(4000);
            entity.HasOne<Generation>().WithMany().HasForeignKey(x => x.GenerationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GenerationOutput>(entity =>
        {
            entity.ToTable("generation_outputs");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.GenerationId);
            entity.Property(x => x.MediaType).HasMaxLength(32);
            entity.Property(x => x.StorageUri).HasMaxLength(2048);
            entity.Property(x => x.MimeType).HasMaxLength(128);
            entity.HasOne<Generation>().WithMany().HasForeignKey(x => x.GenerationId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
