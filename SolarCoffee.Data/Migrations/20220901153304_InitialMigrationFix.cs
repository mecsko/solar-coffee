using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarCoffee.Data.Migrations
{
    public partial class InitialMigrationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerAddress_PrimaryAddressId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerAddress",
                table: "CustomerAddress");

            migrationBuilder.RenameTable(
                name: "CustomerAddress",
                newName: "CustomerAddresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerAddresses",
                table: "CustomerAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerAddresses_PrimaryAddressId",
                table: "Customers",
                column: "PrimaryAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerAddresses_PrimaryAddressId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerAddresses",
                table: "CustomerAddresses");

            migrationBuilder.RenameTable(
                name: "CustomerAddresses",
                newName: "CustomerAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerAddress",
                table: "CustomerAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerAddress_PrimaryAddressId",
                table: "Customers",
                column: "PrimaryAddressId",
                principalTable: "CustomerAddress",
                principalColumn: "Id");
        }
    }
}
