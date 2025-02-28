using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStore9.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig280225 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "Orders",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Orders",
                newName: "Decription");
        }
    }
}
