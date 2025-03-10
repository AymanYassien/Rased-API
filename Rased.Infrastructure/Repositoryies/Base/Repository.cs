using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Base
{
    public class Repository<T, U> : IRepository<T, U> where T : class
    {
        private readonly RasedDbContext _context;
    
    
        public Repository(RasedDbContext context)
        {
            _context = context;
    
        }
    
        // Get all entities as IQueryable for flexible querying
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }
    
        // Get a single entity by its ID (generic type for ID)
        public T GetById(U id)
        {
            return _context.Set<T>().Find(id);
        }
    
        // Add a new entity
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
    
        // Update an existing entity
        public void Update(T entity)
        {
    
            // Attach the entity to the context
            _context.Set<T>().Attach(entity);
    
            // Mark the entity as modified
            _context.Entry(entity).State = EntityState.Modified;
    
            // Save changes to the database
            _context.SaveChanges();
        }
    
        // Delete an entity by ID (generic type for ID)
        public void DeleteById(U id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }
    }
    

    public class Repository_Test<T, TKey> : IRepository_Test<T, TKey> where T : class
{
    private readonly RasedDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository_Test(RasedDbContext context)
    {
        _context = context ;
        _dbSet = _context.Set<T>();
    }
    
    public async Task<IQueryable<T>> GetAllAsync(
        Expression<Func<T, bool>>[]? filters = null,
        Expression<Func<T, object>>[]? includes = null,
        int pageNumber = 0,
        int pageSize = 10)
    {
        IQueryable<T> query =  _dbSet;
        
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

        var items = await query.ToListAsync();

        return query;
    }

    public async Task<T?> GetByIdAsync(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>[]? includes = null,
        bool asNoTracking = true)
    {
        IQueryable<T> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet;

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        
        if (filter != null)
            query = query.Where(filter);

        return await query.FirstOrDefaultAsync();
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

    public void RemoveById(TKey id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
            _dbSet.Remove(entity); 
    }
    
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync(); 
    }
}
}
