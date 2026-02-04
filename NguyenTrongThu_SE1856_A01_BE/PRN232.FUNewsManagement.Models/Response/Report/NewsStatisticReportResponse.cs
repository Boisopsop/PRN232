namespace PRN232.FUNewsManagement.Models.Response.Report
{
    public class NewsStatisticReportResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalNewsArticles { get; set; }
        public List<NewsArticleStatistic> NewsArticles { get; set; } = new List<NewsArticleStatistic>();
    }

    public class NewsArticleStatistic
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;
        public bool NewsStatus { get; set; }
        public string StatusText { get; set; } = string.Empty;
    }
}
