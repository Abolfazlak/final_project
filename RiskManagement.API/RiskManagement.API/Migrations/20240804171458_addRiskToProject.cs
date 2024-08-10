using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class addRiskToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "Risks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Risks_ProjectId",
                table: "Risks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_Projects_ProjectId",
                table: "Risks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_Projects_ProjectId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_ProjectId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Risks");
        }
    }
}
