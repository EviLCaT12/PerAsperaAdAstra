using Domain.Logic.Intefaces;
using Domain.Models;

namespace Domain.UseCases
{
    public class NodeService
    {
        private readonly INodeRepository _db;

        public NodeService(INodeRepository db)
        {
            _db = db;
        }

        public bool AddNode(Node node)
        {
            if (_db.Create(node))
            {
                _db.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<Node> GetAllNodes()
        {
            return _db.GetAll();
        }

        public Node GetNodeByTitle(string title)
        {
            var node = _db.FindByTitle(title);
            if (node is null)
            {
                Console.WriteLine("Вернуло null");
                return null;
            }
            else 
                return node;
        }

        public bool UpdateNode(Node node)
        {
            _db.Update(node);
            return true;
        }

        public bool DeleteNode(int id)
        {
            if (_db.Delete(id))
            {
                _db.Save();
                return true;
            }
            return false;
        }
    }

}