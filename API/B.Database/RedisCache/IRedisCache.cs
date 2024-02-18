

namespace B.Database.RedisCache
{
    public interface IRedisCache
    {
        Task<T> GetData<T>(string key);
        Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);
        Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime, string paginationKey);
        Task<object> RemoveData(string key);
        Task RemoveAllData();
    }
}
