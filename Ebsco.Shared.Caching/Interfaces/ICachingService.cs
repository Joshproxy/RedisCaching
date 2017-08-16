using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebsco.Shared.Caching.Interfaces
{
    public interface ICachingService<T2>
    {
        T Get<T>(string key, Object obj = null) where T : class;
        bool Set<T>(T value, string key, Object obj = null, TimeSpan? expiration = null) where T : class;
        T GetSet<T>(Func<T> getMethod, string key, Object obj = null, TimeSpan? expiration = null) where T : class;
    }
}
