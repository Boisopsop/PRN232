namespace PRN232.FUNewsManagementSystem.API.Models.Response
{
    public class NewsArticleResponse
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? NewsStatus { get; set; }
        public int? CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public int? UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<TagResponse> Tags { get; set; } = new List<TagResponse>();
    }

    public class NewsArticleListResponse
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CategoryName { get; set; }
        public string CreatedByName { get; set; }
        public int? NewsStatus { get; set; }
    }
}