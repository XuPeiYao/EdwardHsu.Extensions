using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.Extensions.Configuration.Redis.StackExchange
{
    public class RedisConfigurationOptions
    {
        public string Prefix { get; set; }
        public int DatabaseIndex { get; set; }

        public static RedisConfigurationOptions Default 
        {
            get
            {
                return new RedisConfigurationOptions()
                {
                    Prefix = string.Empty
                };
            }
        }
    }
}
