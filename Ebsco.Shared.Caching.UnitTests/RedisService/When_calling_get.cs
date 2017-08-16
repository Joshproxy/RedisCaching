using System;
using Ebsco.Shared.Caching.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ebsco.Shared.Caching.UnitTests.RedisService
{
    /// <summary>
    /// Summary description for When_calling_get
    /// </summary>
    [TestClass]
    public class When_calling_get : On_the_redis_service
    {
        private string _stringResponse = "string response";

        private TestFakeClass _testObject = new TestFakeClass
        {
            TestProperty = "USD"
        };

        private Object _wrongShape = new
        {
            InvalidTestProperty = "USD "
        };

        private string _key;
        private TestFakeClass _result;

        private int _intResponse = 42;

        private string _testObjectKey = "test object key";
        private string _nonExistentKey = "nowhere key";
        private string _wrongOjectShapeKey = "wrong shape key";
        private string _throwsExceptionKey = "throw exception key";

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            

            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == getRK(_testObjectKey)), It.IsAny<CommandFlags>()))
                .Returns(JsonConvert.SerializeObject(_testObject));

            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == getRK(_nonExistentKey)), It.IsAny<CommandFlags>()))
                .Returns(RedisValue.Null);

            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == getRK(_wrongOjectShapeKey)), It.IsAny<CommandFlags>()))
                .Returns(JsonConvert.SerializeObject(_wrongOjectShapeKey));

            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == getRK(_throwsExceptionKey)), It.IsAny<CommandFlags>()))
                .Throws(new Exception("Test"));

        }

        public override void Because()
        {
            base.Because();
            _result = RedisService.Get<TestFakeClass>(_key);
        }


        [TestMethod]
        public void Should_return_test_object()
        {
            _key = _testObjectKey;
            Because();
            Assert.AreEqual(_testObject.TestProperty, _result.TestProperty);
        }

        [TestMethod]
        public void Should_return_null_if_key_doesnt_exist()
        {
            _key = _nonExistentKey;
            Because();
            Assert.AreEqual(null, _result);
        }

        [TestMethod]
        public void Should_return_null_if_database_not_set()
        {
            RedisService = new RedisService<TestingNamespacedKey>(null, null);
            Because();
            _key = _testObjectKey;
            Assert.AreEqual(null, _result);
        }

        [TestMethod]
        public void Should_return_null_if_data_cant_be_deserialized()
        {
            _key = _wrongOjectShapeKey;
            Because();
            Assert.AreEqual(null, _result);
        }

        [TestMethod]
        public void Should_return_null_if_exception()
        {
            _key = _throwsExceptionKey;
            Because();
            MockErrorLogger.Verify(m => m.LogError(It.IsAny<Exception>()), Times.Once());
            Assert.AreEqual(null, _result);
        }

        [TestMethod]
        public void Should_return_null_if_logger_exception()
        {
            _key = _throwsExceptionKey;
            MockErrorLogger.Setup(m => m.LogError(It.IsAny<Exception>())).Throws(new Exception("Test"));
            Because();
            Assert.AreEqual(null, _result);
        }
    }
}
