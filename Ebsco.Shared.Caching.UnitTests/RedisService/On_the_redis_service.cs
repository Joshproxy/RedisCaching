using System;
using Ebsco.Shared.Caching.Implementations;
using Ebsco.Shared.Caching.Interfaces;
using Moq;
using StackExchange.Redis;

namespace Ebsco.Shared.Caching.UnitTests.RedisService
{
    public class On_the_redis_service : BaseTest
    {
        protected static string KeyNamespace = "Redis Testing";
        protected string NamespacePrefix = KeyNamespace + ".";

        protected RedisService<TestingNamespacedKey> RedisService;
        protected Mock<IDatabase> MockDatabase = new Mock<IDatabase>();
        protected Mock<IErrorLogger> MockErrorLogger = new Mock<IErrorLogger>();

        public override void Initialize()
        {
            base.Initialize();

            MockErrorLogger.Setup(m => m.LogError(It.IsAny<Exception>())).Verifiable();
            RedisService = new RedisService<TestingNamespacedKey>(MockDatabase.Object, MockErrorLogger.Object);
        }

        public class TestingNamespacedKey : NamespacedKey<TestingNamespacedKey>
        {
            protected override string Namespace { get { return KeyNamespace; } }
        }

        public class TestFakeClass
        {
            public string TestProperty;
        }

        protected string getRK(string key)
        {
            return NamespacePrefix + key;
        }
    }
}
