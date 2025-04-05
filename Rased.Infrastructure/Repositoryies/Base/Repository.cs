using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using System.Linq.Expressions;

namespace Rased.Infrastructure.Repositoryies.Base
{
    public class Repository<T, U> : IRepository<T, U> where T : class
    {
        protected readonly RasedDbContext _context;
        private readonly DbSet<T> _dbSet;

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

        // Update an existing entity asynchronously
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
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
    }


    public class Repository_Test<T, TKey> : IRepository_Test<T, TKey> where T : class
    {
        private readonly RasedDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository_Test(RasedDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IQueryable<T>> GetAllAsync(
            Expression<Func<T, bool>>[]? filters = null,
            Expression<Func<T, object>>[]? includes = null,
            int pageNumber = 0,
            int pageSize = 10)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (filters != null)
                foreach (var filter in filters)
                    query = query.Where(filter);



            int totalCount = await query.CountAsync();
            if (pageSize > 0)
            {
                if (pageSize > 100) pageSize = 100; // Cap page size
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

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
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
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
