namespace PRN232.FUNewsManagement.Models.Response.Common
{
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

        public PaginatedResponse(List<T> items, int count, int page, int pageSize)
        {
            Items = items;
            TotalItems = count;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
