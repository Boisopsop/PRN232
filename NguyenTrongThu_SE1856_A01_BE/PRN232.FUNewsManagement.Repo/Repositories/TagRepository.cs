using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;

namespace PRN232.FUNewsManagement.Repo.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeTagId = null)
        {
            return await _dbSet.AnyAsync(t => 
                t.TagName == name && 
                (!excludeTagId.HasValue || t.TagID != excludeTagId.Value));
        }

        public async Task<bool> HasNewsArticlesAsync(int tagId)
        {
            return await _context.NewsTags.AnyAsync(nt => nt.TagID == tagId);
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _dbSet.OrderBy(t => t.TagName).ToListAsync();
        }
    }
}
