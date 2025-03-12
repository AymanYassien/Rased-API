using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Base
{
    public class Repository<T, U> : IRepository<T, U> where T : class
    {
        protected readonly RasedDbContext _context;

        public Repository(RasedDbContext context)
        {
            _context = context;
        }

        // Get all entities as IQueryable (remains synchronous for flexible querying)
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().AsQueryable();
        }

        // Get a single entity by its ID asynchronously
        public async Task<T?> GetByIdAsync(U id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        // Add a new entity asynchronously
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Update an existing entity asynchronously
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete an entity by ID asynchronously
        public async Task DeleteByIdAsync(U id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
