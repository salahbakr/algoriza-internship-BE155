using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllPaginatedFilteredAsync(Expression<Func<T, bool>> filterCriteria, int page = 1, int pageSize = 5)
        {
            var query = _context.Set<T>().AsQueryable();

            if (filterCriteria != null)
            {
                query = query.Where(filterCriteria);
            }

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByPropertyAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().Where(criteria).ToListAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return entity;
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);

            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);

            return entity;
        }
    }
}