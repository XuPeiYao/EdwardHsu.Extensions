using Microsoft.Extensions.Configuration;

namespace EdwardHsu.Extensions.Configuration.Redis
{
    /// <summary>
    /// Extension methods for adding <see cref="RedisConfigurationProvider"/>
    /// </summary>
    public static class RedisConfigurationExtensions
    {
        /// <summary>
        /// Adds the Redis configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">Redis configuration provider type.</typeparam>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddRedis<T>(this IConfigurationBuilder builder, bool optional = true)
            where T : IRedisAccessProvider
        {
            return builder.Add(new RedisConfigurationSource<T>());
        }


        public static void LoadRedisConfiguration(this IConfigurationRoot configuration, IServiceProvider serviceProvider)
        {
            var providers = configuration.Providers.Where(x=>x is IDIConfigurationProvider);

            foreach (IDIConfigurationProvider provider in providers)
            {
                provider.Load(serviceProvider);
            }
        }
    }
}