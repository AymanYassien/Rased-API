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
        T GetById(U id);
        void Add(T entity);
        void Update(T entity);
        void DeleteById(U id);

    }
    
    public interface IRepository_Test<T, TKey> where T : class
    {
        
        Task<IQueryable<T>> GetAllAsync(
            Expression<Func<T, bool>>[]? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            int pageNumber = 0,
            int pageSize = 10);

        Task<T?> GetByIdAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool asNoTracking = true);

        
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity); 
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        
        Task<int> SaveChangesAsync();
    }
}
