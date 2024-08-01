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
DECLARE
    insert_query TEXT;
    update_query TEXT;
BEGIN
    IF TG_OP = 'INSERT' THEN
        SELECT
            'INSERT INTO ""Nodes"" (' || string_agg(quote_ident(key), ', ') || ') VALUES (' || string_agg(quote_literal(value), ', ') || ');'
        INTO insert_query
        FROM json_each_text(row_to_json(NEW));

        INSERT INTO ""Outboxes"" (""Payload"", ""CreatedDate"", ""Status"")
        VALUES (insert_query, NOW(), 0);
        
        RETURN NEW;
    ELSIF TG_OP = 'UPDATE' THEN
        SELECT
            'UPDATE ""Nodes"" SET ' || string_agg(quote_ident(key) || ' = ' || quote_literal(value), ', ') || ' WHERE id = ' || quote_literal(NEW.id) || ';'
        INTO update_query
        FROM json_each_text(row_to_json(NEW));
        
        INSERT INTO ""Outboxes"" (""Payload"", ""CreatedDate"", ""Status"")
        VALUES (update_query, NOW(), 0);
        
        RETURN NEW;
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO ""Outboxes"" (""Payload"", ""CreatedDate"", ""Status"")
        VALUES (
            'DELETE FROM ""Nodes"" WHERE id = ' || quote_literal(OLD.id) || ';',
            NOW(),
            0
        );
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
