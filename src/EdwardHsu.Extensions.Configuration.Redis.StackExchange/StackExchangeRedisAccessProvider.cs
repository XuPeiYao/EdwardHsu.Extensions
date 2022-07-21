using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using StackExchange.Redis;

namespace EdwardHsu.Extensions.Configuration.Redis.StackExchange
{
    public class StackExchangeRedisAccessProvider:IRedisAccessProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly IServer _server;
        private readonly RedisConfigurationOptions _options;
        private readonly ConfigurationReloadToken _reloadToken;
        private ISubscriber _subscriber;
        public StackExchangeRedisAccessProvider(IServiceProvider serviceProvider, IOptions<RedisConfigurationOptions> optional)
        {
            var connectionMultiplexer = serviceProvider.GetService<ConnectionMultiplexer>() ??
                                        serviceProvider.GetService<IConnectionMultiplexer>();
            _connectionMultiplexer = connectionMultiplexer;
            _options = optional?.Value ?? RedisConfigurationOptions.Default;
            _database = connectionMultiplexer.GetDatabase(_options.DatabaseIndex);
            _server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().FirstOrDefault());
            _reloadToken = new ConfigurationReloadToken();
        }

        public bool TryGet(string key, out string value)
        {
            var _key = GetRealKey(key);
            if (_database.KeyExists(_key))
            {
                value = _database.StringGet(_key);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public void Set(string key, string value)
        {
            var _key = GetRealKey(key);
            _database.StringSet(_key, value);
        }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        public void Load()
        {

        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            var _key = GetRealKey(parentPath + ":*");
            return _server.Keys(_options.DatabaseIndex, _key).Select(x=>x.ToString()).ToArray();
        }

        private string GetRealKey(string key)
        {
            return _options.Prefix + key;
        }
    }
}