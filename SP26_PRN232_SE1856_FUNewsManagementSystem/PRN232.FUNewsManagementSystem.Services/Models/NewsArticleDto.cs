using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagementSystem.Services.Models
{
    public class NewsArticleDto
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public int? CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public int? NewsStatus { get; set; }
        public int? CreatedById { get; set; }
        public SystemAccountDto CreatedBy { get; set; }
        public int? UpdatedById { get; set; }
        public SystemAccountDto UpdatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class TagDto
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string Note { get; set; }
    }

    public class SystemAccountDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public int? AccountRole { get; set; }
    }
}
