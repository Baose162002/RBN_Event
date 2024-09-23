using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        EntityState GetEntityState(T entity);
        IQueryable<T> Get();
        Task<T> GetByIdAsync(Guid code);
        void PrepareCreate(T entity);
        void PrepareUpdate(T entity);
        void PrepareRemove(T entity);
        int Save();
        Task<int> SaveAsync();
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        void Create(T entity);
        Task<int> CreateAsync(T entity);
        void Update(T entity);
        Task<int> UpdateAsync(T entity);
        bool Remove(T entity);
        Task<bool> RemoveAsync(T entity);
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        T GetById(string code);
        Task<T> GetByIdAsync(string code);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> include = null, int? pageIndex = null, int? pageSize = null);
    }
}
