using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public interface ISystemAccountService
    {
        List<SystemAccount> GetAllAccounts();
        SystemAccount? GetAccountById(short id);
        SystemAccount? GetAccountByEmail(string email);
        SystemAccount? AuthenticateUser(string email, string password);
        void CreateAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        bool DeleteAccount(short id);
        List<SystemAccount> SearchAccounts(string? name, string? email, int? role);
    }

    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _repository;

        public SystemAccountService(ISystemAccountRepository repository)
        {
            _repository = repository;
        }

        public List<SystemAccount> GetAllAccounts()
        {
            return _repository.GetAll().ToList();
        }

        public SystemAccount? GetAccountById(short id)
        {
            return _repository.GetById(id);
        }

        public SystemAccount? GetAccountByEmail(string email)
        {
            return _repository.GetByEmail(email);
        }

        public SystemAccount? AuthenticateUser(string email, string password)
        {
            return _repository.AuthenticateUser(email, password);
        }

        public void CreateAccount(SystemAccount account)
        {
            _repository.Add(account);
            _repository.SaveChanges();
        }

        public void UpdateAccount(SystemAccount account)
        {
            _repository.Update(account);
            _repository.SaveChanges();
        }

        public bool DeleteAccount(short id)
        {
            if (!_repository.CanDeleteAccount(id))
            {
                return false; // Cannot delete account with news articles
            }

            var account = _repository.GetById(id);
            if (account != null)
            {
                _repository.Delete(account);
                _repository.SaveChanges();
                return true;
            }
            return false;
        }

        public List<SystemAccount> SearchAccounts(string? name, string? email, int? role)
        {
            var query = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a => a.AccountName!.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(a => a.AccountEmail!.Contains(email));
            }

            if (role.HasValue)
            {
                query = query.Where(a => a.AccountRole == role.Value);
            }

            return query.ToList();
        }
    }
}
