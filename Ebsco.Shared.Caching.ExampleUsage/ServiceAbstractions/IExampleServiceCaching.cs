using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ebsco.Shared.Caching.Interfaces;

namespace Ebsco.Shared.Caching.ExampleUsage.ServiceAbstractions
{
    public interface IExampleServiceCaching : ICachingService<ExampleNamespacedKey>
    {
    }
}
