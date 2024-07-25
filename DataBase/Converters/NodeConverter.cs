using DataBase.Models;
using Domain.Models;

namespace DataBase.Converters
{
    public static class NodeConverter
    {
        public static NodeModel ToModel(this Node model)
        {
            return new NodeModel
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                CreatedDate = model.CreatedDate
            };
        }

        public static Node ToDomain(this NodeModel model)
        {
            return new Node
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                CreatedDate = model.CreatedDate
            };
        }
    }
}