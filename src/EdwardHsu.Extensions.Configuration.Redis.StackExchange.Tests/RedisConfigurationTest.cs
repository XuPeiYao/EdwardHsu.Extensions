using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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



            #region Mock
            var mockServer = new Mock<IServer>();
            mockServer
                .Setup(x => x.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(), It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<CommandFlags>()))
                .Returns(new RedisKey[] { new RedisKey("TEST:ID"), new RedisKey("TEST:NAME") });


            var mockDatabase = new Mock<IDatabase>();
            mockDatabase
                .Setup(x => x.StringGet(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns("NEW");
            mockDatabase
                .Setup(x => x.KeyExists(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns(true);

            var mockMultiplexer = new Mock<IConnectionMultiplexer>();
            mockMultiplexer
                .Setup(x => x.GetEndPoints(It.IsAny<bool>()))
                .Returns(new EndPoint[]{IPEndPoint.Parse("127.0.0.1:80")});
            mockMultiplexer
                .Setup(x => x.GetServer(It.IsAny<EndPoint>(), null))
                .Returns(mockServer.Object);
            mockMultiplexer
                .Setup(x => x.GetDatabase(It.IsAny<int>(), null))
                .Returns(mockDatabase.Object);

            #endregion

            var services = new ServiceCollection();
            services.AddSingleton(mockMultiplexer.Object);
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
