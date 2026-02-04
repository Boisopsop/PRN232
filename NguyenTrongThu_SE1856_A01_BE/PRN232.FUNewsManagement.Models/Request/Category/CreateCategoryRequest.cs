using System.ComponentModel.DataAnnotations;

namespace PRN232.FUNewsManagement.Models.Request.Category
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name must not exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description must not exceed 250 characters")]
        public string CategoryDesciption { get; set; } = string.Empty;

        public short? ParentCategoryID { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
