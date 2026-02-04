namespace FUNewsManagementSystem.Models.Common
{
    /// <summary>
    /// Paginated response wrapper containing pagination metadata
    /// </summary>
    /// <typeparam name="T">Type of items in the collection</typeparam>
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResponse()
        {
        }

        public PaginatedResponse(List<T> items, int totalItems, int page, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }
}
