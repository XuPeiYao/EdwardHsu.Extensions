using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EdwardHsu.Extensions.Configuration.Redis.StackExchange.Tests
{
    public class RedisConfigurationTest
    {
        [Fact]
        public void Test1()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                ["MESSAGE"] = "OLD"
            });
            builder.AddRedis<StackExchangeRedisAccessProvider>();

            
            var configuration = builder.Build();
            



            var services = new ServiceCollection();
            services.AddSingleton(sp =>
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
                return redis;
            });
            services.AddSingleton<StackExchangeRedisAccessProvider>();
            services.AddOptions<RedisConfigurationOptions>().Configure(x =>
            {
                x.Prefix = "CONFIG_";
            });

            var serviceProvider = services.BuildServiceProvider();


            configuration.LoadRedisConfiguration(serviceProvider);

            //var kk = configuration["MESSAGE"];

            var pp = configuration.GetSection("TEST").Get<TEST>();

            Assert.Equal("NEW", configuration["MESSAGE"]);
        }

        public class TEST
        {
            public string ID { get; set; }
            public string NAME { get; set; }
        }
    }
}
