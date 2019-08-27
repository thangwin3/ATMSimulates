using Microsoft.EntityFrameworkCore.Migrations;

namespace ATM.Simulates.API.Migrations
{
    public partial class TraceLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLock",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_WalletTypeId",
                table: "Wallets",
                column: "WalletTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_WalletType_WalletTypeId",
                table: "Wallets",
                column: "WalletTypeId",
                principalTable: "WalletType",
                principalColumn: "WalletTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_WalletType_WalletTypeId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_WalletTypeId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "isLock",
                table: "Accounts");
        }
    }
}
