using Domain.UseCases;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace PerAsperaAdAstra
{
    [ApiController]
    [Route("node")]
    public class NodeController : ControllerBase
    {
        private readonly NodeService _service;
        public NodeController(NodeService nodeService)
        {
            _service = nodeService;
        }

        [HttpPost("create")]
        public IActionResult CreateNode(string title, string content, DateTime createdDate)
        {
            Node node = new(0, title, content, createdDate);

            return Ok(_service.AddNode(node));

        }

        [HttpGet("get_all")]
        public IActionResult GetAllNodes()
        {
            return Ok(_service.GetAllNodes());
        }

        [HttpPut("update")]
        public IActionResult UpdateNode(string old_title, string new_title, string content, DateTime createdDate)
        {
            var node = _service.GetNodeByTitle(old_title);
            Node new_node = new(node.Id, new_title, content, createdDate);
            return Ok(_service.UpdateNode(new_node));
        }

        [HttpDelete("delete")]
        public IActionResult DeleteNode(int id)
        {
            return Ok(_service.DeleteNode(id));
        }
    }
}