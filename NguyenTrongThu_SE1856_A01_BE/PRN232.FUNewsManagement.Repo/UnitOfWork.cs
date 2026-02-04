using Microsoft.EntityFrameworkCore.Storage;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Repo.Repositories;

namespace PRN232.FUNewsManagement.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FUNewsManagementContext _context;
        private IDbContextTransaction? _transaction;

        private ISystemAccountRepository? _systemAccounts;
        private ICategoryRepository? _categories;
        private INewsArticleRepository? _newsArticles;
        private ITagRepository? _tags;

        public UnitOfWork(FUNewsManagementContext context)
        {
            _context = context;
        }

        public ISystemAccountRepository SystemAccounts
        {
            get
            {
                _systemAccounts ??= new SystemAccountRepository(_context);
                return _systemAccounts;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                _categories ??= new CategoryRepository(_context);
                return _categories;
            }
        }

        public INewsArticleRepository NewsArticles
        {
            get
            {
                _newsArticles ??= new NewsArticleRepository(_context);
                return _newsArticles;
            }
        }

        public ITagRepository Tags
        {
            get
            {
                _tags ??= new TagRepository(_context);
                return _tags;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
