using Domain.Logic.Intefaces;
using Domain.Models;
using DataBase.Converters;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository
{
    public class NodeRepository : INodeRepository
    {
        private readonly ApplicationContext _context;

        public NodeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool Create(Node node)
        {
            _context.Nodes.Add(node.ToModel());
            return true;
        }

        public IEnumerable<Node> GetAll()
        {
            return _context.Nodes.AsNoTracking().Select(node => node.ToDomain());
        }

        public Node FindByTitle(string title)
        {
            var node = _context.Nodes.AsNoTracking().FirstOrDefault(node => node.Title == title);
            if (node == default)
                return null;
            return node?.ToDomain();
        }

        public bool Delete(int id)
        {
            var node = _context.Nodes.AsNoTracking().FirstOrDefault(n => n.Id == id);
            if (node == default)
                return false;

            _context.Nodes.Remove(node);
            return true;
        }

        public bool Update(Node node)
        {
            _context.Nodes.Update(node.ToModel());
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
