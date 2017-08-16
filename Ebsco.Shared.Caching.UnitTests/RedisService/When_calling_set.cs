using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StackExchange.Redis;
using Ebsco.Shared.Caching.Implementations;

namespace Ebsco.Shared.Caching.UnitTests.RedisService
{
    /// <summary>
    /// Summary description for When_calling_get_net_title_markup
    /// </summary>
    [TestClass]
    public class When_calling_set : On_the_redis_service
    {
        private bool _result;

        private readonly TestFakeClass _testObject = new TestFakeClass()
        {
            TestProperty = "USD"
        };
        
        private string _key;
        private TestFakeClass _value;
        
        private string _testObjectKey = "test object key";
        private string _failKey = "fail key";
        private string _throwsExceptionKey = "error key";

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            MockDatabase.Setup(m => m.StringSet(
                It.Is<RedisKey>(k => k == getRK(_failKey)),
                    It.Is<RedisValue>(x => x.IsNull),
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .Returns(false);

            MockDatabase.Setup(m => m.StringSet(
                    It.Is<RedisKey>(k => k == getRK(_testObjectKey)),
                    It.Is<RedisValue>(x => x != RedisValue.Null),
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .Returns(true);

            MockDatabase.Setup(m => m.StringSet(
                    It.Is<RedisKey>(k => k == getRK(_throwsExceptionKey)),
                    It.Is<RedisValue>(x => !x.IsNull),
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .Throws(new Exception("Test"));
        }

        public override void Because()
        {
            base.Because();
            _result = RedisService.Set(_value, _key);
        }
        

        [TestMethod]
        public void Should_return_true()
        {
            _key = _testObjectKey;
            _value = _testObject;
            Because();
            Assert.IsTrue(_result);
        }

        [TestMethod]
        public void Should_return_false_if_value_is_null()
        {
            _key = _testObjectKey;
            Because();
            Assert.IsFalse(_result);
        }

        [TestMethod]
        public void Should_return_false_if_database_fails()
        {
            RedisService = new RedisService<TestingNamespacedKey>(null, null);
            _key = _failKey;
            _value = _testObject;
            Because();
            Assert.IsFalse(_result);
        }

        [TestMethod]
        public void Should_return_false_if_exception_thrown()
        {
            _key = _throwsExceptionKey;
            _value = _testObject;
            Because();
            MockErrorLogger.Verify(m => m.LogError(It.IsAny<Exception>()), Times.Once());
            Assert.IsFalse(_result);
        }

        [TestMethod]
        public void Should_return_false_if_logger_exception()
        {
            _key = _throwsExceptionKey;
            MockErrorLogger.Setup(m => m.LogError(It.IsAny<Exception>())).Throws(new Exception("Test"));
            Because();
            Assert.IsFalse(_result);
        }
    }
}
