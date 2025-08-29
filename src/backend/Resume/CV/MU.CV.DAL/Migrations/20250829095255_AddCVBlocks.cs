using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MU.CV.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCVBlocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cv",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerFullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    About = table.Column<string>(type: "text", nullable: true),
                    UniquePath = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cv", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "job_experience",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CvId = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionDescription = table.Column<string>(type: "text", nullable: true),
                    LengthOfWorkFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    LengthOfWorkTo = table.Column<DateOnly>(type: "date", nullable: true),
                    PositionTitle = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    JobCompanyName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    JobCompanyLogo = table.Column<string>(type: "text", nullable: true),
                    IsIT = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsFreelance = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_experience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_job_experience_cv_CvId",
                        column: x => x.CvId,
                        principalTable: "cv",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cv_UniquePath",
                table: "cv",
                column: "UniquePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_experience_CvId",
                table: "job_experience",
                column: "CvId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_experience");

            migrationBuilder.DropTable(
                name: "cv");
        }
    }
}
