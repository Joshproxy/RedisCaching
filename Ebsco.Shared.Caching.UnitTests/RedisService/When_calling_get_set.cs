using System;
using Ebsco.Shared.Caching.Implementations;
using Ebsco.Shared.Caching.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ebsco.Shared.Caching.UnitTests.RedisService
{
    /// <summary>
    /// Summary description for When_calling_get_net_title_markup
    /// </summary>
    [TestClass]
    public class When_calling_get_set : On_the_redis_service
    {
        private TestFakeClass _result;

        private readonly TestFakeClass _testObject = new TestFakeClass
        {
            TestProperty = "USD"
        };

        private string _key;

        private string _testObjectKey = "currency code key";
        private string _failKey = "fail key";
        private string _getExceptionKey = "get error key";
        private string _setExceptionKey = "set error key";
        private string _nonExistentKey = "nowhere key";
        private string _wrongOjectShapeKey = "wrong shape key";

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == getRK(_testObjectKey)), It.IsAny<CommandFlags>()))
                .Returns(JsonConvert.SerializeObject(_testObject));
            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == NamespacePrefix + _nonExistentKey), It.IsAny<CommandFlags>()))
                .Returns(RedisValue.Null);

            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == getRK(_wrongOjectShapeKey)), It.IsAny<CommandFlags>()))
                .Returns(JsonConvert.SerializeObject(_wrongOjectShapeKey));

            MockDatabase.Setup(x => x.StringGet(It.Is<RedisKey>(k =>
                    k == getRK(_getExceptionKey)), It.IsAny<CommandFlags>()))
                .Throws(new Exception("Test"));

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
                    It.Is<RedisKey>(k => k == getRK(_setExceptionKey)),
                    It.Is<RedisValue>(x => !x.IsNull),
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .Throws(new Exception("Test"));
        }

        public override void Because()
        {
            base.Because();
            _result = RedisService.GetSet(() => _testObject, _key);
        }


        [TestMethod]
        public void Should_return_currency_code()
        {
            _key = _testObjectKey;
            Because();
            Assert.AreEqual(_testObject.TestProperty, _result.TestProperty);
        }

        [TestMethod]
        public void Should_return_currency_code_if_key_doesnt_exist()
        {
            _key = _nonExistentKey;
            Because();
            Assert.AreEqual(_testObject.TestProperty, _result.TestProperty);
        }

        [TestMethod]
        public void Should_return_currency_code_if_database_not_set()
        {
            RedisService = new RedisService<TestingNamespacedKey>(null, null);
            _key = _testObjectKey;
            Because();
            Assert.AreEqual(_testObject.TestProperty, _result.TestProperty);
        }

        [TestMethod]
        public void Should_return_currency_code_if_data_cant_be_deserialized()
        {
            _key = _wrongOjectShapeKey;
            Because();
            Assert.AreEqual(_testObject.TestProperty, _result.TestProperty);
        }

        [TestMethod]
        public void Should_return_currency_code_if_get_exception()
        {
            _key = _getExceptionKey;
            Because();
            Assert.AreEqual(_testObject.TestProperty, _result.TestProperty);
        }

        [TestMethod]
        public void Should_return_currency_code_if_set_exception()
        {
            _key = _setExceptionKey;
            Because();
            Assert.AreEqual(_testObject.TestProperty, _result.TestProperty);
        }
    }
}
