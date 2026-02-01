using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository;

namespace PRN232.FUNewsManagementSystem.Repo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        INewsArticleRepository NewsArticleRepository { get; }
        ISystemAccountRepository SystemAccountRepository { get; }
        ITagRepository TagRepository { get; }

        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> SaveAsync();
        int Save();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
