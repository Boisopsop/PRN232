namespace PRN232.FUNewsManagement.Models.Response.Category
{
    public class CategoryDetailResponse
    {
        public short CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDesciption { get; set; } = string.Empty;
        public short? ParentCategoryID { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public int TotalNewsArticles { get; set; }
        public List<CategoryResponse> ChildCategories { get; set; } = new List<CategoryResponse>();
    }
}
