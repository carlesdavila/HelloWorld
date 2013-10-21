using System.Web;
using System.Web.Mvc;

namespace CloudDataAnalytics.IdP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}