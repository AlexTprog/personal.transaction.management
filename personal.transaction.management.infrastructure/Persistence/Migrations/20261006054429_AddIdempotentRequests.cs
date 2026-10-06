using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace personal.transaction.management.infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIdempotentRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "idempotent_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    request_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    response = table.Column<string>(type: "jsonb", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_idempotent_requests", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_idempotent_requests_created_on_utc",
                table: "idempotent_requests",
                column: "created_on_utc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "idempotent_requests");
        }
    }
}
