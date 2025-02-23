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
}
