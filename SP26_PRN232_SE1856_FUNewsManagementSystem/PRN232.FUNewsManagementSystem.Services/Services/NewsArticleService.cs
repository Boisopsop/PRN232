using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Repo.UnitOfWork;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.Services.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<NewsArticle>> GetAllNewsArticlesAsync()
        {
            return await _unitOfWork.NewsArticleRepository.GetAllAsync();
        }

        public async Task<NewsArticle> GetNewsArticleByIdAsync(int id)
        {
            return await _unitOfWork.NewsArticleRepository.GetByIdAsync(id);
        }

        public async Task<NewsArticle> GetNewsArticleWithDetailsAsync(int id)
        {
            return await _unitOfWork.NewsArticleRepository.GetNewsArticleWithDetailsAsync(id);
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.NewsArticleRepository.GetNewsArticlesByCategoryAsync(categoryId);
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByStatusAsync(int status)
        {
            return await _unitOfWork.NewsArticleRepository.GetNewsArticlesByStatusAsync(status);
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByAuthorAsync(int authorId)
        {
            return await _unitOfWork.NewsArticleRepository.GetNewsArticlesByAuthorAsync(authorId);
        }

        public async Task<List<NewsArticle>> GetRecentNewsArticlesAsync(int count)
        {
            return await _unitOfWork.NewsArticleRepository.GetRecentNewsArticlesAsync(count);
        }

        public async Task<bool> CreateNewsArticleAsync(NewsArticle newsArticle)
        {
            try
            {
                newsArticle.CreatedDate = DateTime.Now;
                await _unitOfWork.NewsArticleRepository.CreateAsync(newsArticle);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateNewsArticleAsync(NewsArticle newsArticle)
        {
            try
            {
                newsArticle.ModifiedDate = DateTime.Now;
                await _unitOfWork.NewsArticleRepository.UpdateAsync(newsArticle);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteNewsArticleAsync(int id)
        {
            try
            {
                var newsArticle = await _unitOfWork.NewsArticleRepository.GetByIdAsync(id);
                if (newsArticle == null)
                    return false;

                await _unitOfWork.NewsArticleRepository.RemoveAsync(newsArticle);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
