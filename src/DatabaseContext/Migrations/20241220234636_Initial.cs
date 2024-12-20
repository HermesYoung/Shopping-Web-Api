using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseContext.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shopping_web");

            migrationBuilder.CreateTable(
                name: "category",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    content_json = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    price = table.Column<int>(type: "int", nullable: false),
                    is_sold_out = table.Column<bool>(type: "bit", nullable: false),
                    is_disabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    content_json = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    display_content = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    title = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("promotion_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_category",
                schema: "shopping_web",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_category_pk", x => new { x.product_id, x.category_id });
                    table.ForeignKey(
                        name: "product_category_category_id_fk",
                        column: x => x.category_id,
                        principalSchema: "shopping_web",
                        principalTable: "category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "product_category_product_id_fk",
                        column: x => x.product_id,
                        principalSchema: "shopping_web",
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_sell",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_sell_pk", x => x.id);
                    table.ForeignKey(
                        name: "product_sell_order_id_fk",
                        column: x => x.order_id,
                        principalSchema: "shopping_web",
                        principalTable: "order",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "product_sell_product_id_fk",
                        column: x => x.product_id,
                        principalSchema: "shopping_web",
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_promotion",
                schema: "shopping_web",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    promotion_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_promotion_pk", x => new { x.product_id, x.promotion_id });
                    table.ForeignKey(
                        name: "product_promotion_product_id_fk",
                        column: x => x.product_id,
                        principalSchema: "shopping_web",
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "product_promotion_promotion_id_fk",
                        column: x => x.promotion_id,
                        principalSchema: "shopping_web",
                        principalTable: "promotion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "category_name_index",
                schema: "shopping_web",
                table: "category",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "order_create_date_index",
                schema: "shopping_web",
                table: "order",
                column: "create_date");

            migrationBuilder.CreateIndex(
                name: "order_name_email_phone_index",
                schema: "shopping_web",
                table: "order",
                columns: new[] { "name", "email", "phone" });

            migrationBuilder.CreateIndex(
                name: "order_status_index",
                schema: "shopping_web",
                table: "order",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "product_is_disabled_index",
                schema: "shopping_web",
                table: "product",
                column: "is_disabled");

            migrationBuilder.CreateIndex(
                name: "product_is_visible_index",
                schema: "shopping_web",
                table: "product",
                column: "is_sold_out");

            migrationBuilder.CreateIndex(
                name: "IX_product_category_category_id",
                schema: "shopping_web",
                table: "product_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_promotion_promotion_id",
                schema: "shopping_web",
                table: "product_promotion",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_sell_order_id",
                schema: "shopping_web",
                table: "product_sell",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_sell_product_id",
                schema: "shopping_web",
                table: "product_sell",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "product_sell_date_index",
                schema: "shopping_web",
                table: "product_sell",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "promotion_end_date_index",
                schema: "shopping_web",
                table: "promotion",
                column: "end_date");

            migrationBuilder.CreateIndex(
                name: "promotion_start_date_index",
                schema: "shopping_web",
                table: "promotion",
                column: "start_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_category",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "product_promotion",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "product_sell",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "category",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "promotion",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "order",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "product",
                schema: "shopping_web");
        }
    }
}
