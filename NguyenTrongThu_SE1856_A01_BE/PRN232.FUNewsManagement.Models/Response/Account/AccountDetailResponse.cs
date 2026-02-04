namespace PRN232.FUNewsManagement.Models.Response.Account
{
    public class AccountDetailResponse
    {
        public short AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int AccountRole { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int TotalNewsArticles { get; set; }
    }
}
