using PRN232.FUNewsManagement.Models.Response.Report;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NewsStatisticReportResponse> GetNewsStatisticByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var newsArticles = await _unitOfWork.NewsArticles.GetNewsStatisticByPeriodAsync(startDate, endDate);

            return new NewsStatisticReportResponse
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalNewsArticles = newsArticles.Count(),
                NewsArticles = newsArticles.Select(n => new NewsArticleStatistic
                {
                    NewsArticleID = n.NewsArticleID,
                    NewsTitle = n.NewsTitle,
                    CreatedDate = n.CreatedDate,
                    CategoryName = n.Category?.CategoryName ?? string.Empty,
                    CreatedByName = n.CreatedBy?.AccountName ?? string.Empty,
                    NewsStatus = n.NewsStatus,
                    StatusText = n.NewsStatus ? "Active" : "Inactive"
                }).ToList()
            };
        }
    }
}
