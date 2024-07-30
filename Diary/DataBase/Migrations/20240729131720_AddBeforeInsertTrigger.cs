using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AddBeforeInsertTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // создание функции-обработчика
            migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION trg_func_nodes() RETURNS TRIGGER AS $$
            BEGIN
                IF TG_OP = 'INSERT' THEN
                    INSERT INTO ""Outboxes"" (""Payload"", ""CreatedDate"", ""Status"")
                    VALUES ('INSERTED: ' || row_to_json(NEW)::TEXT, NOW(), 0);
                    RETURN NEW;
                ELSIF TG_OP = 'UPDATE' THEN
                    INSERT INTO ""Outboxes"" (""Payload"", ""CreatedDate"", ""Status"")
                    VALUES ('UPDATED: ' || row_to_json(NEW)::TEXT, NOW(), 0);
                    RETURN NEW;
                ELSIF TG_OP = 'DELETE' THEN
                    INSERT INTO ""Outboxes"" (""Payload"", ""CreatedDate"", ""Status"")
                    VALUES ('DELETED: ' || row_to_json(OLD)::TEXT, NOW(), 0);
                    RETURN OLD;
                END IF;
                RETURN NULL;
            END;
            $$ LANGUAGE plpgsql;
        ");

            // Создание триггера на таблице nodes
            migrationBuilder.Sql(@"
            CREATE TRIGGER trg_after_change_nodes
            AFTER INSERT OR UPDATE OR DELETE ON ""Nodes""
            FOR EACH ROW EXECUTE FUNCTION trg_func_nodes();
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаление триггера и функции
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_after_change_nodes ON ""Nodes"";");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS trg_func_nodes;");
        }
    }
}
