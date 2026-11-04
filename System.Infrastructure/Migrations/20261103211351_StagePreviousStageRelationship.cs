using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StagePreviousStageRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stages_PrevClassID",
                table: "Stages",
                column: "PrevClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Stages_PrevClassID",
                table: "Stages",
                column: "PrevClassID",
                principalTable: "Stages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Stages_PrevClassID",
                table: "Stages");

            migrationBuilder.DropIndex(
                name: "IX_Stages_PrevClassID",
                table: "Stages");
        }
    }
}
