#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace BookInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Category_Book",
                table: "Category",
                column: "bookId",
                principalTable: "Book",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseBook_Book",
                table: "WarehouseBook",
                column: "bookId",
                principalTable: "Book",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order",
                table: "Payment",
                column: "orderId",
                principalTable: "Order",
                principalColumn: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingBasketBook_Book",
                table: "ShoppingBasketBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingBasketBook_ShoppingBasket",
                table: "ShoppingBasketBook",
                column: "shoppingBasketID",
                principalTable: "ShoppingBasket",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Book",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseBook_Book",
                table: "WarehouseBook");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingBasketBook_Book",
                table: "ShoppingBasketBook");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingBasketBook_ShoppingBasket",
                table: "ShoppingBasketBook");
        }
    }
}