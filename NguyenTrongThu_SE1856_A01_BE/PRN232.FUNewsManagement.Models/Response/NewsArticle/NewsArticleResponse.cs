namespace PRN232.FUNewsManagement.Models.Response.NewsArticle
{
    public class NewsArticleResponse
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public short CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool NewsStatus { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public short CreatedByID { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime? ModifiedDate { get; set; }
    }
}
