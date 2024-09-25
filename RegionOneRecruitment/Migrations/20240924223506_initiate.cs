using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegionOneRecruitment.Migrations
{
    /// <inheritdoc />
    public partial class InitiateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OpeningId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "Openings",
                columns: table => new
                {
                    OpeningId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: true),
                    JobDescription = table.Column<string>(type: "TEXT", nullable: true),
                    PayRate = table.Column<string>(type: "TEXT", nullable: true),
                    SkillsRequired = table.Column<string>(type: "TEXT", nullable: true),
                    CertificationsRequired = table.Column<string>(type: "TEXT", nullable: true),
                    PositionFilled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PostedDate = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Openings", x => x.OpeningId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    JobInterestsAsString = table.Column<string>(type: "TEXT", nullable: true), // Changed here
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    ResumeBytes = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Openings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
