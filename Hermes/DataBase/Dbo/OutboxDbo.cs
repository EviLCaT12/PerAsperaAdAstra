namespace DataBase.Dbo;

public class OutboxDbo
{
    public int Id { get; set; }
    public string Payload { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public int Status { get; set; } = 0;
}
