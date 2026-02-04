namespace FUNewsManagementSystem.Models.Responses
{
    /// <summary>
    /// Response model for Category - used in API responses
    /// </summary>
    public class CategoryResponse
    {
        public short CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDesciption { get; set; }
        public short? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool? IsActive { get; set; }
    }
}
