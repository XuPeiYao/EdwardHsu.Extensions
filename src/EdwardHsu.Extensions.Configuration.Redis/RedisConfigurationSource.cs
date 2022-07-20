using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EdwardHsu.Extensions.Configuration.Redis
{
    public class RedisConfigurationSource<T> : IConfigurationSource
        where T : IRedisAccessProvider
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RedisConfigurationProvider<T>();
        }
    }
}