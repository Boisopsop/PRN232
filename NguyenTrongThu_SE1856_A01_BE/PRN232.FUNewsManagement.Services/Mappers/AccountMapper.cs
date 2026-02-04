using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Models.Enums;
using PRN232.FUNewsManagement.Models.Request.Account;
using PRN232.FUNewsManagement.Models.Response.Account;

namespace PRN232.FUNewsManagement.Services.Mappers
{
    public static class AccountMapper
    {
        public static AccountResponse ToResponse(SystemAccount entity)
        {
            return new AccountResponse
            {
                AccountID = entity.AccountID,
                AccountName = entity.AccountName,
                AccountEmail = entity.AccountEmail,
                AccountRole = entity.AccountRole,
                RoleName = ((AccountRole)entity.AccountRole).ToString()
            };
        }

        public static AccountDetailResponse ToDetailResponse(SystemAccount entity, int totalNewsArticles = 0)
        {
            return new AccountDetailResponse
            {
                AccountID = entity.AccountID,
                AccountName = entity.AccountName,
                AccountEmail = entity.AccountEmail,
                AccountRole = entity.AccountRole,
                RoleName = ((AccountRole)entity.AccountRole).ToString(),
                TotalNewsArticles = totalNewsArticles
            };
        }

        public static SystemAccount ToEntity(CreateAccountRequest request, string hashedPassword)
        {
            return new SystemAccount
            {
                AccountName = request.AccountName,
                AccountEmail = request.AccountEmail,
                AccountRole = request.AccountRole,
                AccountPassword = hashedPassword
            };
        }

        public static void UpdateEntity(SystemAccount entity, UpdateAccountRequest request, string? hashedPassword = null)
        {
            entity.AccountName = request.AccountName;
            entity.AccountEmail = request.AccountEmail;
            entity.AccountRole = request.AccountRole;
            
            if (!string.IsNullOrEmpty(hashedPassword))
            {
                entity.AccountPassword = hashedPassword;
            }
        }
    }
}
