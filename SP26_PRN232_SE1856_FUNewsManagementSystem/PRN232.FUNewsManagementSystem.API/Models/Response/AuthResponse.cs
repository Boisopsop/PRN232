namespace PRN232.FUNewsManagementSystem.API.Models.Response
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public DateTime ExpiresAt { get; set; }
        public SystemAccountResponse User { get; set; }
    }

    public class SystemAccountResponse
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public int? AccountRole { get; set; }
    }
}