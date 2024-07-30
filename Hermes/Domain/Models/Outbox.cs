namespace Domain.Models;

public class OutboxDbo
{
    public int Id { get; set; }
    public string Payload { get; set; }
    public DateTime createdDate { get; set; }
    public Status status{ get; set; }
}

public enum Status
{
    Waiting, //Только пришла в outbox
    OnFlight, //Гермес забрал с таблицы
    Fail, //Не удалось отправить в кафку.
}