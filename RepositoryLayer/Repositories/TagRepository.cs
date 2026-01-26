using RepositoryLayer.Entities;
using System.Linq;

namespace RepositoryLayer.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Tag? GetByName(string tagName);
    }

    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public Tag? GetByName(string tagName)
        {
            return _dbSet.FirstOrDefault(t => t.TagName == tagName);
        }
    }
}
