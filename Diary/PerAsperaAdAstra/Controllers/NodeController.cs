using Domain.UseCases;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace PerAsperaAdAstra
{
    public class NodeController : ControllerBase
    {
        private readonly NodeService _service;
        public NodeController(NodeService nodeService)
        {
            _service = nodeService;
        }

        [HttpPost("create")]
        public IActionResult CreateNode(string title, string content)
        {
            Node node = Node.CreateNode(title, content);

            return Ok(_service.AddNode(node));

        }

        [HttpGet("get_all")]
        public IActionResult GetAllNodes()
        {
            return Ok(_service.GetAllNodes());
        }

        [HttpPut("update")]
        public IActionResult UpdateNode(string old_title, string new_title, string content)
        {
            var node = _service.GetNodeByTitle(old_title);
            node.Title = new_title;
            node.Content = content;
            return Ok(_service.UpdateNode(node));
        }

        [HttpDelete("delete")]
        public IActionResult DeleteNode(int id)
        {
            return Ok(_service.DeleteNode(id));
        }
    }
}