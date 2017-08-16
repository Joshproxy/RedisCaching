using System.Web;
using System.Web.Mvc;

namespace Ebsco.Shared.Caching.ExampleUsage
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
