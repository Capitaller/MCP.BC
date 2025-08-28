using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Configuration;
using MCP.BusinessCentral.Infrastructure;

namespace MCP.BusinessCentral.Triggers
{
    internal class GetSpecificEndpoint
    {
        private const string ToolName = "call_specific_bc_entity";
        private const string ToolDescription = "Makes request to specific Business Central entity to retrieve data";
        private const string ToolPropEntityName = "Entity Name";
        private const string ToolPropEntityDescription = "The BC entity to retrieve (e.g., 'employees')";
        private readonly IConfiguration _configuration;

        public GetSpecificEndpoint(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Function("get_entity")]
        public async Task<IActionResult> Run(
            [McpToolTrigger(ToolName, ToolDescription)] ToolInvocationContext context,
            [McpToolProperty(ToolPropEntityName, "string", ToolPropEntityDescription)] string? entityName
        )
        {
            try
            {
                using var bcClient = new Client(_configuration);
                var json = await bcClient.GetAsync(entityName ?? string.Empty);

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