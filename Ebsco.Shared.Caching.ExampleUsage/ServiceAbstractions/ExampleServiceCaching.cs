using Ebsco.Shared.Caching.Implementations;
using Ebsco.Shared.Caching.Interfaces;
using StackExchange.Redis;

namespace Ebsco.Shared.Caching.ExampleUsage.ServiceAbstractions
{
    public class ExampleServiceCaching : RedisService<ExampleNamespacedKey>, IExampleServiceCaching
    {
        public ExampleServiceCaching(IDatabase redisDb, IErrorLogger errorLogger) : base(redisDb, errorLogger) { }
    }
    
    public class ExampleNamespacedKey : NamespacedKey<ExampleNamespacedKey>
    {
        protected override string Namespace { get { return "ExampleUsage.Service"; } }
    }
}