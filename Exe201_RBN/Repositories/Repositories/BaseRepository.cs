using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

            private readonly ApplicationDBContext _context;
            private readonly DbSet<T> _dbSet;

            public BaseRepository()
            {
                _context ??= new ApplicationDBContext();
                _dbSet ??= _context.Set<T>();
            }

            #region Separating asign entity and save operators

            public BaseRepository(ApplicationDBContext context)
            {
                _context = context;
                _dbSet = _context.Set<T>();
            }

            public EntityState GetEntityState(T entity)
            {
                return _context.Entry(entity).State;
            }

            #endregion Separating asign entity and save operators


            public async Task<T> GetByIdAsync(Guid code)
            {
                return await _context.Set<T>().FindAsync(code);
            }
            #region Separating asign entity and save operators


            public void PrepareCreate(T entity)
            {
                _context.Add(entity);
            }

            public void PrepareUpdate(T entity)
            {
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;
            }

            public void PrepareRemove(T entity)
            {
                _context.Remove(entity);
            }

            public int Save()
            {
                return _context.SaveChanges();
            }

            public async Task<int> SaveAsync()
            {
                return await _context.SaveChangesAsync();
            }

            #endregion Separating asign entity and save operators


            public List<T> GetAll()
            {
                return _context.Set<T>().ToList();
            }
            public async Task<List<T>> GetAllAsync()
            {
                return await _context.Set<T>().ToListAsync();
            }
            public void Create(T entity)
            {
                _context.Add(entity);
                _context.SaveChanges();
            }

            public async Task<int> CreateAsync(T entity)
            {
                await _context.AddAsync(entity);
                return await _context.SaveChangesAsync();
            }

            public void Update(T entity)
            {
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;
                _context.SaveChanges();
            }

            public async Task<int> UpdateAsync(T entity)
            {
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;

                return await _context.SaveChangesAsync();
            }

            public bool Remove(T entity)
            {
                _context.Remove(entity);
                _context.SaveChanges();
                return true;
            }

            public async Task<bool> RemoveAsync(T entity)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }

            public T GetById(int id)
            {
                return _context.Set<T>().Find(id);
            }

            public async Task<T> GetByIdAsync(int id)
            {
                return await _context.Set<T>().FindAsync(id);
            }

            public T GetById(string code)
            {
                return _context.Set<T>().Find(code);
            }

            public async Task<T> GetByIdAsync(string code)
            {
                return await _context.Set<T>().FindAsync(code);
            }

            public IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> include = null, int? pageIndex = null, int? pageSize = null)
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (include != null)
                {
                    query = include(query);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (pageIndex.HasValue && pageSize.HasValue)
                {
                    int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                    int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

                    query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
                }

                return query;
            }
        }
}

