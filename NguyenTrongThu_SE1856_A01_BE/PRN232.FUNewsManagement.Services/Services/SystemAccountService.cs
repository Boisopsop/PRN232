using PRN232.FUNewsManagement.Models.Request.Account;
using PRN232.FUNewsManagement.Models.Response.Account;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Helpers;
using PRN232.FUNewsManagement.Services.Interfaces;
using PRN232.FUNewsManagement.Services.Mappers;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountDetailResponse?> GetByIdAsync(short id)
        {
            var account = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            if (account == null)
            {
                return null;
            }

            var totalNews = await _unitOfWork.NewsArticles.CountAsync(n => n.CreatedByID == id);
            return AccountMapper.ToDetailResponse(account, totalNews);
        }

        public async Task<PaginatedResponse<AccountResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            int? role = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            var (items, totalCount) = await _unitOfWork.SystemAccounts.GetPagedAsync(
                page,
                pageSize,
                searchTerm,
                role,
                sortBy,
                isDescending);

            var responseItems = items.Select(AccountMapper.ToResponse).ToList();

            return new PaginatedResponse<AccountResponse>(
                responseItems,
                totalCount,
                page,
                pageSize);
        }

        public async Task<AccountDetailResponse> CreateAsync(CreateAccountRequest request)
        {
            // Check email exists
            if (await _unitOfWork.SystemAccounts.EmailExistsAsync(request.AccountEmail))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var hashedPassword = PasswordHelper.HashPassword(request.AccountPassword);
            var account = AccountMapper.ToEntity(request, hashedPassword);

            await _unitOfWork.SystemAccounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return AccountMapper.ToDetailResponse(account, 0);
        }

        public async Task<AccountDetailResponse> UpdateAsync(short id, UpdateAccountRequest request)
        {
            var account = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            if (account == null)
            {
                throw new InvalidOperationException("Account not found");
            }

            // Check email exists (excluding current account)
            if (await _unitOfWork.SystemAccounts.EmailExistsAsync(request.AccountEmail, id))
            {
                throw new InvalidOperationException("Email already exists");
            }

            string? hashedPassword = null;
            if (!string.IsNullOrEmpty(request.AccountPassword))
            {
                hashedPassword = PasswordHelper.HashPassword(request.AccountPassword);
            }

            AccountMapper.UpdateEntity(account, request, hashedPassword);

            _unitOfWork.SystemAccounts.Update(account);
            await _unitOfWork.SaveChangesAsync();

            var totalNews = await _unitOfWork.NewsArticles.CountAsync(n => n.CreatedByID == id);
            return AccountMapper.ToDetailResponse(account, totalNews);
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var account = await _unitOfWork.SystemAccounts.GetByIdAsync(id);
            if (account == null)
            {
                return false;
            }

            // Check if account has created news articles
            if (await _unitOfWork.SystemAccounts.HasCreatedNewsArticlesAsync(id))
            {
                throw new InvalidOperationException("Cannot delete account with created news articles");
            }

            _unitOfWork.SystemAccounts.Delete(account);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null)
        {
            return await _unitOfWork.SystemAccounts.EmailExistsAsync(email, excludeAccountId);
        }
    }
}
