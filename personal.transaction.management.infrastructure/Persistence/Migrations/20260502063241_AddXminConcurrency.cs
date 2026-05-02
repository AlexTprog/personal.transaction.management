using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace personal.transaction.management.infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddXminConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // xmin is a PostgreSQL system column present on every table - no DDL needed
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // xmin is a PostgreSQL system column - cannot be dropped
        }
    }
}
