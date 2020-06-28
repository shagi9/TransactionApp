using Microsoft.EntityFrameworkCore.Migrations;

namespace TA.Data.Migrations
{
    public partial class withtypemoney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "ClientName", "Status", "Type" },
                values: new object[] { 1, 100.12m, "Ilya Shagoferov", "Pending", "Refill" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "ClientName", "Status", "Type" },
                values: new object[] { 2, 200.20m, "Vanya Efimov", "Completed", "Withdrawal" });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "ClientName", "Status", "Type" },
                values: new object[] { 3, 300.50m, "Roma Nagalevskiy", "Cancelled", "Refill" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
