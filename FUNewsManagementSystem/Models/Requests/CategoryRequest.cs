using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Models.Requests
{
    public class CreateCategoryRequest
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string CategoryDesciption { get; set; } = string.Empty;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; } = true;
    }

    public class UpdateCategoryRequest
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string CategoryDesciption { get; set; } = string.Empty;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }
    }
}
