namespace Api.Cache
{
    public interface IRedisService
    {
        public T GetCachedData<T>(string key);
        public void SetCachedData<T>(string key, T data, TimeSpan cacheDuration);
    }
}