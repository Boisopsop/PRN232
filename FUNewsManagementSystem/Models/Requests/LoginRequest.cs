using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUNewsManagementSystem.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("accountEmail")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("accountPassword")]
        public string Password { get; set; } = string.Empty;
    }
}
