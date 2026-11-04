using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SessionParentOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_SessionID",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SessionID",
                table: "Subjects",
                column: "SessionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_SessionID",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SessionID",
                table: "Subjects",
                column: "SessionID",
                unique: true,
                filter: "[SessionID] IS NOT NULL");
        }
    }
}
