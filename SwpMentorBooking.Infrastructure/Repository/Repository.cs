using Microsoft.EntityFrameworkCore;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Infrastructure.Data;
using System.Linq;
using System.Linq.Expressions;

namespace SwpMentorBooking.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }
        public T Get(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter is not null)
            {   // Apply the filter to the query
                query = query.Where(filter);
            }
            // includeProperties should be case-sensitive
            if (!string.IsNullOrEmpty(includeProperties)) // includeProperties is not null or empty
            {   // Split the string by comma and remove empty entries
                foreach (var property in includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {   // Include the related entities
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter is not null)
            {   // Apply the filter to the query
                query = query.Where(filter);
            }
            // includeProperties should be case-sensitive
            if (!string.IsNullOrEmpty(includeProperties)) // includeProperties is not null or empty
            {   // Split the string by comma and remove empty entries
                foreach (var property in includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {   // Include the related entities
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }
        public T Add(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public T Delete(T entity)
        {
            dbSet.Remove(entity);
            return entity;
        }
    }
}
