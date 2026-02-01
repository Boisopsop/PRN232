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
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SystemAccount>> GetAllAccountsAsync()
        {
            return await _unitOfWork.SystemAccountRepository.GetAllAsync();
        }

        public async Task<SystemAccount> GetAccountByIdAsync(int id)
        {
            return await _unitOfWork.SystemAccountRepository.GetByIdAsync(id);
        }

        public async Task<SystemAccount> GetByEmailAsync(string email)
        {
            return await _unitOfWork.SystemAccountRepository.GetByEmailAsync(email);
        }

        public async Task<SystemAccount> LoginAsync(string email, string password)
        {
            return await _unitOfWork.SystemAccountRepository.GetByEmailAndPasswordAsync(email, password);
        }

        public async Task<List<SystemAccount>> GetAccountsByRoleAsync(int role)
        {
            return await _unitOfWork.SystemAccountRepository.GetAccountsByRoleAsync(role);
        }

        public async Task<bool> CreateAccountAsync(SystemAccount account)
        {
            try
            {
                if (await _unitOfWork.SystemAccountRepository.IsEmailExistAsync(account.AccountEmail))
                    return false;

                await _unitOfWork.SystemAccountRepository.CreateAsync(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAccountAsync(SystemAccount account)
        {
            try
            {
                await _unitOfWork.SystemAccountRepository.UpdateAsync(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            try
            {
                var account = await _unitOfWork.SystemAccountRepository.GetByIdAsync(id);
                if (account == null)
                    return false;

                await _unitOfWork.SystemAccountRepository.RemoveAsync(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _unitOfWork.SystemAccountRepository.IsEmailExistAsync(email);
        }
    }
}
