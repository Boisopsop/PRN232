using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Models.Requests
{
    public class CreateNewsArticleRequest
    {
        [Required]
        [StringLength(20)]
        public string NewsArticleId { get; set; } = string.Empty;

        [StringLength(400)]
        public string? NewsTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string Headline { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? NewsContent { get; set; }

        [StringLength(400)]
        public string? NewsSource { get; set; }

        [Required]
        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; } = true;

        public List<int> TagIds { get; set; } = new List<int>();
    }

    public class UpdateNewsArticleRequest
    {
        [StringLength(400)]
        public string? NewsTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string Headline { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? NewsContent { get; set; }

        [StringLength(400)]
        public string? NewsSource { get; set; }

        [Required]
        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
    }
}
