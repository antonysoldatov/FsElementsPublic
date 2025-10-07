using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FsElements.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_361 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElementCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElementForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElementCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElementForms_ElementCategories_ElementCategoryId",
                        column: x => x.ElementCategoryId,
                        principalTable: "ElementCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceWholesale = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceRetail = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    ElementFormId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Elements_AspNetUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Elements_ElementCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ElementCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elements_ElementForms_ElementFormId",
                        column: x => x.ElementFormId,
                        principalTable: "ElementForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementForms_ElementCategoryId",
                table: "ElementForms",
                column: "ElementCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_CategoryId",
                table: "Elements",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ElementFormId",
                table: "Elements",
                column: "ElementFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_SellerId",
                table: "Elements",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "ElementForms");

            migrationBuilder.DropTable(
                name: "ElementCategories");
        }
    }
}
