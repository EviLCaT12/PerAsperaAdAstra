using Domain.Models;

namespace Domain.Logic.Intefaces;

public interface INodeRepository : IRepository<Node>
{
    public Node FindByTitle(string title);
}