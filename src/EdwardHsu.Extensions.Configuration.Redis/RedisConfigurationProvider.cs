using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace EdwardHsu.Extensions.Configuration.Redis
{
    public class RedisConfigurationProvider<T> : IConfigurationProvider, IDIConfigurationProvider
        where T : IRedisAccessProvider
    {
        private T _redisAccessProvider;

        public RedisConfigurationProvider()
        {
        }

        public bool TryGet(string key, out string value)
        {
            return _redisAccessProvider.TryGet(key, out value);
        }

        public void Set(string key, string value)
        {
            _redisAccessProvider.Set(key, value);
        }

        public IChangeToken GetReloadToken()
        {
            return null;
        }

        public void Load()
        {
            
        }

        public void Load(IServiceProvider serviceProvider)
        {
            _redisAccessProvider = (T)serviceProvider.GetService(typeof(T));
            _redisAccessProvider.Load();
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            return _redisAccessProvider.GetChildKeys(earlierKeys, parentPath);
        }
    }
}