using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class addCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_SecondaryRiskCategory_SecondaryRiskCategoryId",
                table: "Risks");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryRiskCategory_MainRiskCategory_MainRiskCategoryId",
                table: "SecondaryRiskCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecondaryRiskCategory",
                table: "SecondaryRiskCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainRiskCategory",
                table: "MainRiskCategory");

            migrationBuilder.RenameTable(
                name: "SecondaryRiskCategory",
                newName: "SecondaryRiskCategories");

            migrationBuilder.RenameTable(
                name: "MainRiskCategory",
                newName: "MainRiskCategories");

            migrationBuilder.RenameIndex(
                name: "IX_SecondaryRiskCategory_MainRiskCategoryId",
                table: "SecondaryRiskCategories",
                newName: "IX_SecondaryRiskCategories_MainRiskCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecondaryRiskCategories",
                table: "SecondaryRiskCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainRiskCategories",
                table: "MainRiskCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_SecondaryRiskCategories_SecondaryRiskCategoryId",
                table: "Risks",
                column: "SecondaryRiskCategoryId",
                principalTable: "SecondaryRiskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryRiskCategories_MainRiskCategories_MainRiskCategoryId",
                table: "SecondaryRiskCategories",
                column: "MainRiskCategoryId",
                principalTable: "MainRiskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Risks_SecondaryRiskCategories_SecondaryRiskCategoryId",
                table: "Risks");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryRiskCategories_MainRiskCategories_MainRiskCategoryId",
                table: "SecondaryRiskCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecondaryRiskCategories",
                table: "SecondaryRiskCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MainRiskCategories",
                table: "MainRiskCategories");

            migrationBuilder.RenameTable(
                name: "SecondaryRiskCategories",
                newName: "SecondaryRiskCategory");

            migrationBuilder.RenameTable(
                name: "MainRiskCategories",
                newName: "MainRiskCategory");

            migrationBuilder.RenameIndex(
                name: "IX_SecondaryRiskCategories_MainRiskCategoryId",
                table: "SecondaryRiskCategory",
                newName: "IX_SecondaryRiskCategory_MainRiskCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecondaryRiskCategory",
                table: "SecondaryRiskCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainRiskCategory",
                table: "MainRiskCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_SecondaryRiskCategory_SecondaryRiskCategoryId",
                table: "Risks",
                column: "SecondaryRiskCategoryId",
                principalTable: "SecondaryRiskCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryRiskCategory_MainRiskCategory_MainRiskCategoryId",
                table: "SecondaryRiskCategory",
                column: "MainRiskCategoryId",
                principalTable: "MainRiskCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
