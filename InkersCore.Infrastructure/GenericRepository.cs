using InkersCore.Domain.IRepositories;
using InkersCore.Domain.IServices;
using InkersCore.Infrastructure.Configurations;
using InkersCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InkersCore.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private readonly ILoggerService<GenericRepository<T>> _loggerService;
        private DbSet<T> _table;

        public GenericRepository(AppDBContext context, ILoggerService<GenericRepository<T>> loggerService)
        {
            _context = context;
            _table = _context.Set<T>();
            _loggerService = loggerService;
        }

        /// <summary>
        /// Function to get object by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Object</returns>
        public object? GetById(object id)
        {
            return _table.Find(id);
        }

        /// <summary>
        /// Function to insert object
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>object</returns>
        public object Insert(T obj)
        {
            _table.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        /// <summary>
        /// Function to insert object list
        /// </summary>
        /// <param name="objList">objList</param>
        /// <returns>ObjectList</returns>
        public List<T> InsertRange(List<T> objList)
        {
            _table.AddRange(objList);
            _context.SaveChanges();
            return objList.ToList();
        }

        /// <summary>
        /// Function to update object
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>object</returns>
        public object Update(T obj)
        {
            _table.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        /// <summary>
        /// Function to update object list
        /// </summary>
        /// <param name="objList">objList</param>
        /// <returns>ObjectList</returns>
        public List<T> UpdateRange(List<T> objList)
        {
            _table.AddRange(objList);
            _context.SaveChanges();
            return objList;
        }

        /// <summary>
        /// Function to find entity records based on filter
        /// </summary>
        /// <param name="filter">EntityFilter</param>
        /// <returns>EntityList</returns>
        IEnumerable<T> IGenericRepository<T>.Find(EntityFilter<T> filter)
        {
            var query = _context.Set<T>().Where(filter.Predicate);

            if (filter.SortBy != null)
            {
                query = filter.SortAscending ? query.OrderBy(filter.SortBy) : query.OrderByDescending(filter.SortBy);
            }

            return filter.RowCount > 0 ? query.Take((int)filter.RowCount).ToList() : query.ToList();
        }
    }
}
