using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Models;

namespace PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<List<NewsArticle>> GetNewsArticlesByCategoryAsync(int categoryId);
        Task<List<NewsArticle>> GetNewsArticlesByStatusAsync(int status);
        Task<NewsArticle> GetNewsArticleWithDetailsAsync(int id);
        Task<List<NewsArticle>> GetNewsArticlesByAuthorAsync(int authorId);
        Task<List<NewsArticle>> GetRecentNewsArticlesAsync(int count);
    }
}
