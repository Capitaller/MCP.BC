using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Configuration;
using MCP.BusinessCentral.Infrastructure;

namespace MCP.BusinessCentral.Triggers
{
    public class GetAvailableEntitySets
    {
        private const string ToolName = "get_available_entity_sets";
        private const string ToolDescription = "List available Business Central Entity Sets to select the most suitable one.";
        private readonly IConfiguration _configuration;

        public GetAvailableEntitySets(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Function("get_available_entity_sets")]
        public async Task<IActionResult> Run(
            [McpToolTrigger(ToolName, ToolDescription)] ToolInvocationContext context)
        {
            try
            {
                using var bcClient = new Client(_configuration);
                var json = await bcClient.GetAsync();

                return new ContentResult
                {
                    Content = json,
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
            catch (HttpRequestException)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}