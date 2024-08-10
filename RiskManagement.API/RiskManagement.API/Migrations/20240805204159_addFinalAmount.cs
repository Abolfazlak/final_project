using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class addFinalAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FinalAmount",
                table: "Risks",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Risks");
        }
    }
}
