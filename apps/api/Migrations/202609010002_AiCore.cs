using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

public partial class AiCore : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(name: "ai_providers", columns: table => new
        {
            Id = table.Column<Guid>(type: "uuid", nullable: false), Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false), DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false), Enabled = table.Column<bool>(type: "boolean", nullable: false)
        }, constraints: table => table.PrimaryKey("PK_ai_providers", x => x.Id));
        migrationBuilder.CreateIndex(name: "IX_ai_providers_Key", table: "ai_providers", column: "Key", unique: true);

        migrationBuilder.CreateTable(name: "ai_models", columns: table => new
        {
            Id = table.Column<Guid>(type: "uuid", nullable: false), ProviderId = table.Column<Guid>(type: "uuid", nullable: false), ExternalModelId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false), DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false), MediaType = table.Column<int>(type: "integer", nullable: false), Enabled = table.Column<bool>(type: "boolean", nullable: false), Priority = table.Column<int>(type: "integer", nullable: false), MaxConcurrency = table.Column<int>(type: "integer", nullable: false), Capabilities = table.Column<string>(type: "jsonb", nullable: false), Metadata = table.Column<string>(type: "jsonb", nullable: false)
        }, constraints: table => { table.PrimaryKey("PK_ai_models", x => x.Id); table.ForeignKey(name: "FK_ai_models_ai_providers_ProviderId", column: x => x.ProviderId, principalTable: "ai_providers", principalColumn: "Id", onDelete: ReferentialAction.Restrict); });
        migrationBuilder.CreateIndex(name: "IX_ai_models_ProviderId_ExternalModelId", table: "ai_models", columns: new[] { "ProviderId", "ExternalModelId" }, unique: true);

        migrationBuilder.CreateTable(name: "generations", columns: table => new
        {
            Id = table.Column<Guid>(type: "uuid", nullable: false), ModelId = table.Column<Guid>(type: "uuid", nullable: false), UserId = table.Column<Guid>(type: "uuid", nullable: true), Mode = table.Column<int>(type: "integer", nullable: false), Status = table.Column<int>(type: "integer", nullable: false), Prompt = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false), Settings = table.Column<string>(type: "jsonb", nullable: false), SourceAssetIds = table.Column<string>(type: "jsonb", nullable: false), IdempotencyKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true), ExternalJobId = table.Column<string>(type: "text", nullable: true), ProviderRequestId = table.Column<string>(type: "text", nullable: true), ErrorCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true), ErrorMessage = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true), AttemptCount = table.Column<int>(type: "integer", nullable: false), CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false), UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false), CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
        }, constraints: table => table.PrimaryKey("PK_generations", x => x.Id));
        migrationBuilder.CreateIndex(name: "IX_generations_IdempotencyKey", table: "generations", column: "IdempotencyKey", unique: true, filter: "\"idempotency_key\" IS NOT NULL");
        migrationBuilder.CreateIndex(name: "IX_generations_Status_CreatedAt", table: "generations", columns: new[] { "Status", "CreatedAt" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "generations");
        migrationBuilder.DropTable(name: "ai_models");
        migrationBuilder.DropTable(name: "ai_providers");
    }
}
