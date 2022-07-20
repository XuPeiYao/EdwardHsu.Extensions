using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdwardHsu.Extensions.Configuration.Redis
{
    public interface IDIConfigurationProvider
    {
        void Load(IServiceProvider serviceProvider);
    }
}
