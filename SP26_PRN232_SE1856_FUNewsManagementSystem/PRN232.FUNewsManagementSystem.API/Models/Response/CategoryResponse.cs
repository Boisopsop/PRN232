namespace PRN232.FUNewsManagementSystem.API.Models.Response
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public bool? IsActive { get; set; }
        public List<CategoryResponse> ChildCategories { get; set; } = new List<CategoryResponse>();
    }
}