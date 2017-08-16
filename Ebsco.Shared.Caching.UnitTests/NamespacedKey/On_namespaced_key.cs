using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ebsco.Shared.Caching.Interfaces;

namespace Ebsco.Shared.Caching.UnitTests.NamespacedKey
{
    [TestClass]
    public class On_namespaced_key : BaseTest
    {

        public override void Initialize()
        {
            base.Initialize();

            
        }
    }

    public class TestNamespacedKey : NamespacedKey<TestNamespacedKey>
    {
        protected override string Namespace { get { return "Testing.UnitTests"; } }
    }
}
