namespace FUNewsManagementSystem.Models.Responses
{
    public class LoginResponse
    {
        public short AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int? AccountRole { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
