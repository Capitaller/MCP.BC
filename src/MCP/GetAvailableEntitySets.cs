using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using MCP.BusinessCentral.Infrastructure;
using System.Text.Json.Nodes;

namespace MCP.BusinessCentral.Triggers
{
    public class GetAvailableEntitySets
    {
        private const string ToolName = "get_available_entity_sets";
        private const string ToolDescription = "List available Business Central Entity Sets to select the most suitable one.";
        private readonly Client _client;
        public GetAvailableEntitySets(Client client)
        {
            _client = client;
        }

        [Function("get_available_entity_sets")]
        public async Task<IActionResult> Run(
            [McpToolTrigger(ToolName, ToolDescription)] ToolInvocationContext context)
        {
                var json = await _client.GetAsync();
                return new OkObjectResult(JsonNode.Parse(json));
        }
    }
}