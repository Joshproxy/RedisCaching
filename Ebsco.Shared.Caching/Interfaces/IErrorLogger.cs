using System;
using System.Collections.Generic;
using System.Text;

namespace Ebsco.Shared.Caching.Interfaces
{
    public interface IErrorLogger
    {
        void LogError(Exception ex);
        void LogError(string message, Exception ex);
    }
}
