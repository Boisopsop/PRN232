using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Repo.Repositories;
using PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository;

namespace PRN232.FUNewsManagementSystem.Repo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FUNewsDBContext _context;
        private IDbContextTransaction? _transaction;
        private readonly Dictionary<Type, object> _repositories;

        private ICategoryRepository? _categoryRepository;
        private INewsArticleRepository? _newsArticleRepository;
        private ISystemAccountRepository? _systemAccountRepository;
        private ITagRepository? _tagRepository;

        public UnitOfWork(FUNewsDBContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                _categoryRepository ??= new CategoryRepository(_context);
                return _categoryRepository;
            }
        }

        public INewsArticleRepository NewsArticleRepository
        {
            get
            {
                _newsArticleRepository ??= new NewsArticleRepository(_context);
                return _newsArticleRepository;
            }
        }

        public ISystemAccountRepository SystemAccountRepository
        {
            get
            {
                _systemAccountRepository ??= new SystemAccountRepository(_context);
                return _systemAccountRepository;
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                _tagRepository ??= new TagRepository(_context);
                return _tagRepository;
            }
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<T>(_context);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Save()
        {
            return _context.SaveChanges();
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
        }
    }
}
