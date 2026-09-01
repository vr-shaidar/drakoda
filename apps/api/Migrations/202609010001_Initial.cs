using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "system_settings",
            columns: table => new
            {
                Key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                Value = table.Column<string>(type: "jsonb", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_system_settings", x => x.Key));
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "system_settings");
}
