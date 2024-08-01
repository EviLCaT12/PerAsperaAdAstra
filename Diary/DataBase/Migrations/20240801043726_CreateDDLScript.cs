using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class CreateDDLScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION log_ddl_event() RETURNS event_trigger AS $$
            DECLARE
                ddl_command text;
            BEGIN
                ddl_command := current_query();  -- Получаем команду DDL
                INSERT INTO ""Outboxes"" (""Payload"", ""CreatedDate"", ""Status"")
                VALUES (ddl_command, NOW(), 0);  -- Записываем команду в таблицу Outboxes
            END;
            $$ LANGUAGE plpgsql;
            ");
            migrationBuilder.Sql(@"
                CREATE EVENT TRIGGER log_ddl_commands ON ddl_command_end
                EXECUTE FUNCTION log_ddl_event();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS log_ddl_commands;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS log_ddl_event;");
        }
    }
}
