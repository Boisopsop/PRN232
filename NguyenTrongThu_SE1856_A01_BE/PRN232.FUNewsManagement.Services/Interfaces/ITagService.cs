using PRN232.FUNewsManagement.Models.Request.Tag;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface ITagService
    {
        Task<TagResponse?> GetByIdAsync(int id);
        Task<List<TagResponse>> GetAllAsync();
        Task<TagResponse> CreateAsync(CreateTagRequest request);
        Task<TagResponse> UpdateAsync(int id, UpdateTagRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
