namespace Domain.Models;

public class Node
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedDate { get; set; }

    public Node(int id, string title, string content, DateTime createdDate)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedDate = createdDate;
    }
    public Node() {}
}