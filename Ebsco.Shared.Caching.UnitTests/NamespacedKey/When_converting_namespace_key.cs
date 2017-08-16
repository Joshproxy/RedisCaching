using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace Ebsco.Shared.Caching.UnitTests.NamespacedKey
{
    [TestClass]
    public class When_converting_namespace_key : On_namespaced_key
    {

        private Object testObject = new
        {
            testParam = "test"
        };

        private string _prefixValue = "[prefix value]";
        private TestNamespacedKey _nsKey;
        private string _expectedValue;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Because()
        {
            base.Because();
            _nsKey = TestNamespacedKey.Create(_prefixValue, testObject);
            _expectedValue = "Testing.UnitTests." + _prefixValue + ".{\"testParam\":\"test\"}";
        }
        
        [TestMethod]
        public void Should_convert_to_string()
        {
            Because();
            Assert.AreEqual(_expectedValue, _nsKey);
        }

        [TestMethod]
        public void Should_convert_to_redis_key()
        {
            Because();
            Assert.AreEqual((RedisKey)_expectedValue, _nsKey);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_error()
        {
            TestNamespacedKey.Create(String.Empty);
        }
            
    }
}
