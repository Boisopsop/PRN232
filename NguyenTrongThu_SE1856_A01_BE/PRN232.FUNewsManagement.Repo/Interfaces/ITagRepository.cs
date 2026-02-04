using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task<bool> NameExistsAsync(string name, int? excludeTagId = null);
        Task<bool> HasNewsArticlesAsync(int tagId);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
    }
}
