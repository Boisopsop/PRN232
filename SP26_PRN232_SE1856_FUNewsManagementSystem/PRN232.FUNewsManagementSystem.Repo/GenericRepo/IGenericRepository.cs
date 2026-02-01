using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagementSystem.Repo.GenericRepo
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetAllQueryable();

        void Create(T entity);
        Task<int> CreateAsync(T entity);
        void Add(T entity);

        void Update(T entity);
        Task<int> UpdateAsync(T entity);

        bool Remove(T entity);
        Task<bool> RemoveAsync(T entity);

        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        T GetById(string code);
        Task<T> GetByIdAsync(string code);
        T GetById(Guid code);
        Task<T> GetByIdAsync(Guid code);

        void PrepareCreate(T entity);
        void PrepareUpdate(T entity);
        void PrepareRemove(T entity);
        int Save();
        Task<int> SaveAsync();
    }
}
