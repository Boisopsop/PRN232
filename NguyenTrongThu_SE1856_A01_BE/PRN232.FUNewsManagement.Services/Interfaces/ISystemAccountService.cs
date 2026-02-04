using PRN232.FUNewsManagement.Models.Request.Account;
using PRN232.FUNewsManagement.Models.Response.Account;
using PRN232.FUNewsManagement.Models.Response.Common;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface ISystemAccountService
    {
        Task<AccountDetailResponse?> GetByIdAsync(short id);
        Task<PaginatedResponse<AccountResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            int? role = null,
            string? sortBy = null,
            bool isDescending = false);
        Task<AccountDetailResponse> CreateAsync(CreateAccountRequest request);
        Task<AccountDetailResponse> UpdateAsync(short id, UpdateAccountRequest request);
        Task<bool> DeleteAsync(short id);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
    }
}
