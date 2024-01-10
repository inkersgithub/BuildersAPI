namespace InkersCore.Domain.IServices
{
    public interface ITokenCacheService
    {
        /// <summary>
        /// Function to insert key/value into cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>Status</returns>
        public bool Insert(string key, string value);

        /// <summary>
        /// Function to remove key/value from cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Status</returns>
        public bool Delete(string key);

        /// <summary>
        /// Function to fetch value from cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public string? FetchValue(string key);
    }
}
