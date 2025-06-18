using System.Linq.Expressions;

namespace Rased.Infrastructure.Repositoryies.Base
{
    public interface IRepository<T, U> where T : class
    {
        IQueryable<T> GetAll();

        // Usual
        IQueryable<T> GetData(
        Expression<Func<T, bool>>[]? filters = null,
        Expression<Func<T, object>>[]? includes = null,
        bool track = true);
        // Generic
        IQueryable<EType> GetData<EType>(
        Expression<Func<EType, bool>>[]? filters = null,
        Expression<Func<EType, object>>[]? includes = null,
        bool track = true) where EType : class;

        Task<T?> GetByIdAsync(U id);
        Task AddAsync(T entity);
        Task AddAsync<M>(M member) where M : class;
        
        Task UpdateAsync(T entity);
        Task UpdateAsync<Key>(Key entity) where Key : class;

        Task DeleteByIdAsync(U id);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);


        void Remove(T entity);
        void Remove<Key>(Key entity) where Key : class;
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
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<int> SaveChangesAsync();
    }
}
