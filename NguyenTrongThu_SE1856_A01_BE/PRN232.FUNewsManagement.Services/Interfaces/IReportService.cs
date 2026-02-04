using PRN232.FUNewsManagement.Models.Response.Report;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface IReportService
    {
        Task<NewsStatisticReportResponse> GetNewsStatisticByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}
