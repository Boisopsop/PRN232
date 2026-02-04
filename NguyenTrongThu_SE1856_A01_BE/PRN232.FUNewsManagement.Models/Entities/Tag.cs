namespace PRN232.FUNewsManagement.Models.Entities
{
    public class Tag
    {
        public int TagID { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
