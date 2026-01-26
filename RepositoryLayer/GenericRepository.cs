using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RepositoryLayer
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly FUNewsManagementContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(FUNewsManagementContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public virtual T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public virtual int Count(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
