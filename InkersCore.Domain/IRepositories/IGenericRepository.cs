using InkersCore.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace InkersCore.Domain.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Function to get transaction Context
        /// </summary>
        /// <returns>IDbContextTransaction</returns>
        public IDbContextTransaction GetContextTransaction();

        /// <summary>
        /// Function to get object by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Object</returns>
        object? GetById(object id);

        /// <summary>
        /// Function to insert object
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>object</returns>
        object Insert(T obj);

        /// <summary>
        /// Function to update object
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>object</returns>
        object Update(T obj);

        /// <summary>
        /// Function to insert object list
        /// </summary>
        /// <param name="objList">objList</param>
        /// <returns>ObjectList</returns>
        List<T> InsertRange(List<T> obj);

        /// <summary>
        /// Function to update object list
        /// </summary>
        /// <param name="objList">objList</param>
        /// <returns>ObjectList</returns>
        List<T> UpdateRange(List<T> obj);

        /// <summary>
        /// Function to find entity records based on filter
        /// </summary>
        /// <param name="filter">EntityFilter</param>
        /// <returns>EntityList</returns>
        IEnumerable<T> Find(EntityFilter<T> filter);
    }
}
