namespace MCP.BusinessCentral.Infrastructure
{
    public class BusinessCentralOptions
    {
        public const string SectionName = "BusinessCentral";

        public string BaseUrl { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
