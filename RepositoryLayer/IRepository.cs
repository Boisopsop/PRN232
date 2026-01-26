using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryLayer
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T? GetById(object id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        bool Exists(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>>? predicate = null);
        void SaveChanges();
    }
}
