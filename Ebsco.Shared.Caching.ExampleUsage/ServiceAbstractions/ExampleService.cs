using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ebsco.Shared.Caching.ExampleUsage.Models;

namespace Ebsco.Shared.Caching.ExampleUsage.ServiceAbstractions
{
    public class ExampleService : IExampleService
    {
        private readonly IExampleServiceCaching _caching;
        public ExampleService(IExampleServiceCaching caching)
        {
            _caching = caching;
        }

        public ExampleClass GetExampleData(string parameter)
        {
            return _caching.GetSet(
                () => CallMockServiceMethod(parameter),
                "GetExampleData",
                parameter,
                new TimeSpan(0, 0, 10));
        }

        public ExampleClass CallMockServiceMethod(string parameter)
        {
            return new ExampleClass() { ExampleIntProperty = 42, ExampleStringProperty = parameter };
        }
    }
}