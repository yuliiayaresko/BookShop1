using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetailsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Order Details",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "customerEmail",
                table: "ShoppingBasket",
                newName: "customerId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingBasket_customerEmail",
                table: "ShoppingBasket",
                newName: "IX_ShoppingBasket_customerId");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "OrderDetails",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "statusId",
                table: "OrderDetails",
                newName: "orderId");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ShoppingBasket",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "orderDate",
                table: "Order",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "orderId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Book",
                type: "nvarchar(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order Details",
                table: "OrderDetails",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Order Details",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "status",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "ShoppingBasket",
                newName: "customerEmail");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingBasket_customerId",
                table: "ShoppingBasket",
                newName: "IX_ShoppingBasket_customerEmail");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderDetails",
                newName: "orderId");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "OrderDetails",
                newName: "statusId");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ShoppingBasket",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "orderDate",
                table: "Order",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "orderId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "Order",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Book",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order Details",
                table: "OrderDetails",
                column: "orderId");
        }
    }
}
