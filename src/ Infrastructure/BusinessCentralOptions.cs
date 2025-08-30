using System.ComponentModel.DataAnnotations;

namespace MCP.BusinessCentral.Infrastructure
{
    public class BusinessCentralOptions
    {
        public const string SectionName = "BusinessCentral";

        [Required]
        [Url]
        public string BaseUrl { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
