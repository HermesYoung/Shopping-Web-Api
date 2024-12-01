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
                name: " purchase_history",
                schema: "shopping_web",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    purchase_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    order_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "category",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    parent = table.Column<int>(type: "int", nullable: true),
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    serial_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_pk", x => x.id);
                    table.UniqueConstraint("AK_order_serial_number", x => x.serial_number);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    price = table.Column<int>(type: "int", nullable: false)
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    content_json = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    title = table.Column<int>(type: "int", nullable: false),
                    display_content = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("promotion_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    password = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_category",
                schema: "shopping_web",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
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
                name: "order_content",
                schema: "shopping_web",
                columns: table => new
                {
                    order_sn = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    promotion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "order_content_order_serial_number_fk",
                        column: x => x.order_sn,
                        principalSchema: "shopping_web",
                        principalTable: "order",
                        principalColumn: "serial_number");
                    table.ForeignKey(
                        name: "order_content_product_id_fk",
                        column: x => x.product,
                        principalSchema: "shopping_web",
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "order_content_promotion_id_fk",
                        column: x => x.promotion,
                        principalSchema: "shopping_web",
                        principalTable: "promotion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_promotion",
                schema: "shopping_web",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    promotion_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
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

            migrationBuilder.CreateTable(
                name: "operation_log",
                schema: "shopping_web",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    operation = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    content = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    status_code = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("operation_log_pk", x => x.id);
                    table.ForeignKey(
                        name: "operation_log_user_id_fk",
                        column: x => x.user_id,
                        principalSchema: "shopping_web",
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "purchase_history_purchase_date_index",
                schema: "shopping_web",
                table: " purchase_history",
                column: "purchase_date");

            migrationBuilder.CreateIndex(
                name: "category_name_index",
                schema: "shopping_web",
                table: "category",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "category_parent_index",
                schema: "shopping_web",
                table: "category",
                column: "parent");

            migrationBuilder.CreateIndex(
                name: "IX_operation_log_user_id",
                schema: "shopping_web",
                table: "operation_log",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "operation_log_date_index",
                schema: "shopping_web",
                table: "operation_log",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "operation_log_operation_index",
                schema: "shopping_web",
                table: "operation_log",
                column: "operation");

            migrationBuilder.CreateIndex(
                name: "order_serial_number_uindex",
                schema: "shopping_web",
                table: "order",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_content_order_sn",
                schema: "shopping_web",
                table: "order_content",
                column: "order_sn");

            migrationBuilder.CreateIndex(
                name: "IX_order_content_product",
                schema: "shopping_web",
                table: "order_content",
                column: "product");

            migrationBuilder.CreateIndex(
                name: "IX_order_content_promotion",
                schema: "shopping_web",
                table: "order_content",
                column: "promotion");

            migrationBuilder.CreateIndex(
                name: "IX_product_category_category_id",
                schema: "shopping_web",
                table: "product_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_category_product_id",
                schema: "shopping_web",
                table: "product_category",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_promotion_product_id",
                schema: "shopping_web",
                table: "product_promotion",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_promotion_promotion_id",
                schema: "shopping_web",
                table: "product_promotion",
                column: "promotion_id");

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

            migrationBuilder.CreateIndex(
                name: "user_pk_2",
                schema: "shopping_web",
                table: "user",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: " purchase_history",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "operation_log",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "order_content",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "product_category",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "product_promotion",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "user",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "order",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "category",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "product",
                schema: "shopping_web");

            migrationBuilder.DropTable(
                name: "promotion",
                schema: "shopping_web");
        }
    }
}
