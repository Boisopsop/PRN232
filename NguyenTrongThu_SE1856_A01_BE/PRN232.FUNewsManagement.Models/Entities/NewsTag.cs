namespace PRN232.FUNewsManagement.Models.Entities
{
    public class NewsTag
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public int TagID { get; set; }
        
        // Navigation properties
        public virtual NewsArticle NewsArticle { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
