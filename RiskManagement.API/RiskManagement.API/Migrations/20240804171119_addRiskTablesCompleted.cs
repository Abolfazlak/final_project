using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class addRiskTablesCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainRiskCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainRiskCategory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SecondaryRiskCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainRiskCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondaryRiskCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecondaryRiskCategory_MainRiskCategory_MainRiskCategoryId",
                        column: x => x.MainRiskCategoryId,
                        principalTable: "MainRiskCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Risks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecondaryRiskCategoryId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FinishedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BestSolutionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risks_SecondaryRiskCategory_SecondaryRiskCategoryId",
                        column: x => x.SecondaryRiskCategoryId,
                        principalTable: "SecondaryRiskCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiskDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RupPhase = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsOpportunity = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RiskProbability = table.Column<int>(type: "int", nullable: true),
                    RiskImpact = table.Column<int>(type: "int", nullable: true),
                    RiskScore = table.Column<int>(type: "int", nullable: true),
                    OpportunityProbability = table.Column<int>(type: "int", nullable: true),
                    OpportunityImpact = table.Column<int>(type: "int", nullable: true),
                    OpportunityScore = table.Column<int>(type: "int", nullable: true),
                    EstimatedRiskAmount = table.Column<long>(type: "bigint", nullable: true),
                    EstimatedOpportunityAmount = table.Column<long>(type: "bigint", nullable: true),
                    DocumentUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstimatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RiskId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskDetails_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    RiskId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solutions_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RiskDetails_RiskId",
                table: "RiskDetails",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_SecondaryRiskCategoryId",
                table: "Risks",
                column: "SecondaryRiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryRiskCategory_MainRiskCategoryId",
                table: "SecondaryRiskCategory",
                column: "MainRiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_RiskId",
                table: "Solutions",
                column: "RiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskDetails");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "Risks");

            migrationBuilder.DropTable(
                name: "SecondaryRiskCategory");

            migrationBuilder.DropTable(
                name: "MainRiskCategory");
        }
    }
}
