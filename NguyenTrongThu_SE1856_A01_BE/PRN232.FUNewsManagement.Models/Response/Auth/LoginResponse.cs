namespace PRN232.FUNewsManagement.Models.Response.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public short AccountId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Role { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
