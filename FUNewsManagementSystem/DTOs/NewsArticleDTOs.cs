using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.DTOs
{
    public class NewsArticleDTO
    {
        public string NewsArticleId { get; set; } = string.Empty;

        [StringLength(400)]
        public string? NewsTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string Headline { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }

        [StringLength(4000)]
        public string? NewsContent { get; set; }

        [StringLength(400)]
        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
    }

    public class CreateNewsArticleDTO
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

    public class UpdateNewsArticleDTO
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
