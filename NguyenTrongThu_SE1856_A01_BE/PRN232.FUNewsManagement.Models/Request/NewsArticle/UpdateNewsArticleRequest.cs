using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.NewsArticle
{
    public class UpdateNewsArticleRequest
    {
        [Required(ErrorMessage = "News title is required")]
        [StringLength(400, ErrorMessage = "News title must not exceed 400 characters")]
        public string NewsTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(150, ErrorMessage = "Headline must not exceed 150 characters")]
        public string Headline { get; set; } = string.Empty;

        [Required(ErrorMessage = "News content is required")]
        public string NewsContent { get; set; } = string.Empty;

        [StringLength(400, ErrorMessage = "News source must not exceed 400 characters")]
        public string NewsSource { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category ID is required")]
        public short CategoryID { get; set; }

        public bool NewsStatus { get; set; } = true;

        public List<int> TagIDs { get; set; } = new List<int>();
    }
}
