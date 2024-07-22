using Domain.Models;

namespace Domain.Logic.Intefaces;

public interface INodeRepository : IRepository<Node>
{
    Node FindByTitle(string title);
}