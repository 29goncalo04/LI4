using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scootlytic.Migrations
{
    /// <inheritdoc />
    public partial class FixUserCarrinhoRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Carrinhos_CartIdCarrinho",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CartIdCarrinho",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CartIdCarrinho",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartIdCarrinho",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CartIdCarrinho",
                table: "Users",
                column: "CartIdCarrinho");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Carrinhos_CartIdCarrinho",
                table: "Users",
                column: "CartIdCarrinho",
                principalTable: "Carrinhos",
                principalColumn: "IdCarrinho",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
