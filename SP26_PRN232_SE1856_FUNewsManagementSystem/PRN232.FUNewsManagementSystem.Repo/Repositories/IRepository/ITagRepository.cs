using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Models;

namespace PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<Tag> GetTagByNameAsync(string tagName);
        Task<List<Tag>> GetTagsByNewsArticleIdAsync(int newsArticleId);
        Task<bool> IsTagNameExistAsync(string tagName);
    }
}
