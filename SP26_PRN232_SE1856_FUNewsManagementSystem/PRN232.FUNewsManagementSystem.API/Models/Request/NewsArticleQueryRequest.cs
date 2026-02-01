using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagementSystem.API.Models.Request
{
    public class CreateNewsArticleRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(255)]
        public string NewsTitle { get; set; }

        [MaxLength(255)]
        public string Headline { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string NewsContent { get; set; }

        [MaxLength(100)]
        public string NewsSource { get; set; }

        public int? CategoryId { get; set; }
        public int? NewsStatus { get; set; }
        public int? CreatedById { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }

    public class UpdateNewsArticleRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(255)]
        public string NewsTitle { get; set; }

        [MaxLength(255)]
        public string Headline { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string NewsContent { get; set; }

        [MaxLength(100)]
        public string NewsSource { get; set; }

        public int? CategoryId { get; set; }
        public int? NewsStatus { get; set; }
        public int? UpdatedById { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }

    public class NewsArticleQueryRequest
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? Status { get; set; }
        public int? CreatedById { get; set; }
        public string SortBy { get; set; } = "createdDate";
        public string SortOrder { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}