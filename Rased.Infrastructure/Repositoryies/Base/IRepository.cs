using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Base
{
    public interface IRepository<T, U> where T : class
    {
        IQueryable<T> GetAll();

        // ---->>> Testing 
        IQueryable<T> GetAll(
        Expression<Func<T, bool>>[]? filters = null,
        Expression<Func<T, object>>[]? includes = null,
        //int pageNumber = 0,
        //int pageSize = 10,
        bool track = true);

        Task<T?> GetByIdAsync(U id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteByIdAsync(U id);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        
    }

    public interface IRepository_Test<T, TKey> where T : class
    {
        
        Task<IQueryable<T>> GetAllAsync(
            Expression<Func<T, bool>>[]? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            int pageNumber = 0,
            int pageSize = 10);

        Task<T?> GetAsync(
            Expression<Func<T, bool>>[]? filters = null,
            Expression<Func<T, object>>[]? includes = null,
            bool asNoTracking = true);


        Task<T?> GetByIdAsync(TKey id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity); 
        void Remove(T entity);
        bool RemoveById(TKey id);
        void RemoveRange(IEnumerable<T> entities);
        
        Task<int> SaveChangesAsync();
    }
}
