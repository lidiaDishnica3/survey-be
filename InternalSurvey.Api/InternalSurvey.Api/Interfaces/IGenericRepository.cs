using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicte, params Expression<Func<T, object>>[] includes);
        Task<T> FindOne(Expression<Func<T, bool>> predicte);
        IQueryable<T> GetAllAsQueryable();
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> GetByStringId(string id);
        IQueryable<T> Query();
        void Remove(T entity);
        Task<bool> SaveChanges();
        Task SaveChangesAsync();
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
    }
}
