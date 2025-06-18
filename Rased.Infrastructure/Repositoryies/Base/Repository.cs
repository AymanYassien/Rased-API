using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using System.Linq.Expressions;

namespace Rased.Infrastructure.Repositoryies.Base
{
    public class Repository<T, U> : IRepository<T, U> where T : class
    {
        protected readonly RasedDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(RasedDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Get all entities as IQueryable (remains synchronous for flexible querying)
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().AsQueryable();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }



        // Overloaded Method
        public IQueryable<T> GetData(
        Expression<Func<T, bool>>[]? filters = null,
        Expression<Func<T, object>>[]? includes = null,
        bool track = true)
        {
            IQueryable<T> query = track ? _dbSet : _dbSet.AsNoTracking();

            if (filters != null)
                foreach (var filter in filters)
                    query = query.Where(filter);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query;
        }
        // Generic Method to get all entities
        public IQueryable<EType> GetData<EType>(
        Expression<Func<EType, bool>>[]? filters = null,
        Expression<Func<EType, object>>[]? includes = null,
        bool track = true) where EType : class
        {
            IQueryable<EType> query = track ? _context.Set<EType>() : _context.Set<EType>().AsNoTracking();

            if (filters != null)
                foreach (var filter in filters)
                    query = query.Where(filter);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query;
        }


        // Get a single entity by its ID asynchronously
        public async Task<T?> GetByIdAsync(U id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Add a new entity asynchronously
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        
        // Generic Method to add Members
        public async Task AddAsync<M>(M member) where M : class
        {
            await _context.Set<M>().AddAsync(member!);
        }

        // Update an existing entity asynchronously
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateAsync<Key>(Key entity) where Key : class
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        // Delete an entity by ID asynchronously
        public async Task DeleteByIdAsync(U id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        // Remove an entity
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Generic Remove
        public void Remove<Key>(Key entity) where Key : class
        {
            _context.Set<Key>().Remove(entity);
        }
    }

    public class Repository_Test<T, TKey> : IRepository_Test<T, TKey> where T : class
    {
        protected readonly RasedDbContext _context;
        protected readonly DbSet<T> _dbSet;
        
    public Repository_Test(RasedDbContext context)
    {
        _context = context ;
        _dbSet = _context.Set<T>();
    }
    
    public async Task<IQueryable<T>> GetAllAsync(
        Expression<Func<T, bool>>[]? filters = null,
        Expression<Func<T, object>>[]? includes = null,
        int pageNumber = 0,
        int pageSize = 0)
    {
        IQueryable<T> query =  _dbSet;
        
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        
        if (filters != null)
            foreach (var filter in filters)
                query = query.Where(filter);
            
        
        
        int totalCount = await query.CountAsync();
        // if (pageSize > 0)
        // {
        //     if (pageSize > 100) pageSize = 100; // Cap page size
        //     query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        // }
        var items = query;

            return query;
        }
    

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>>[]? filters = null,
            Expression<Func<T, object>>[]? includes = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
  

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);


            if (filters != null)
                foreach (var filter in filters)
                    query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> GetByIdAsync(TKey id)
        {
            return _dbSet.Find(id);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            // _context.ChangeTracker.Clear();
            // _context.Set<T>().Attach(entity);
            // _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
            //return true;
 
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public bool RemoveById(TKey id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
                return true;
            }

            return false;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
