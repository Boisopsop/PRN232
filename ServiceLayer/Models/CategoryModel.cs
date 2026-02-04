namespace ServiceLayer.Models
{
    /// <summary>
    /// Business model for Category - used in Service layer
    /// </summary>
    public class CategoryModel
    {
        public short CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDesciption { get; set; }
        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
        
        // Additional navigation properties for detailed responses
        public string? ParentCategoryName { get; set; }
    }
}
