using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ebsco.Shared.Caching.UnitTests
{
    [TestClass]
    public class BaseTest
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            
        }

        [TestInitialize]
        public virtual void Initialize()
        {
        }

        public virtual void Because()
        {
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
        }
    }
}
