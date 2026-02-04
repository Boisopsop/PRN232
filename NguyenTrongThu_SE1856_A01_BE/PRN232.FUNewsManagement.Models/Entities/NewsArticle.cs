namespace PRN232.FUNewsManagement.Models.Entities
{
    public class NewsArticle
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string NewsContent { get; set; } = string.Empty;
        public string NewsSource { get; set; } = string.Empty;
        public short CategoryID { get; set; }
        public bool NewsStatus { get; set; }
        public short CreatedByID { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual SystemAccount CreatedBy { get; set; } = null!;
        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
