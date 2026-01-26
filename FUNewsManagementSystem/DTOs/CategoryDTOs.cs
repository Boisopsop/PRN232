using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.DTOs
{
    public class CategoryDTO
    {
        public short CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string CategoryDesciption { get; set; } = string.Empty;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CreateCategoryDTO
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
}
