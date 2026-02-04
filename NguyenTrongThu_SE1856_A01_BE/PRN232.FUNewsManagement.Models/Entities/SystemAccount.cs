namespace PRN232.FUNewsManagement.Models.Entities
{
    public class SystemAccount
    {
        public short AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int AccountRole { get; set; }
        public string AccountPassword { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();
    }
}
