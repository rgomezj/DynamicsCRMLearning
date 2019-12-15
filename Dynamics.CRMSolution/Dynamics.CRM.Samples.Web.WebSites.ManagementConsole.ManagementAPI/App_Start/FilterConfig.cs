using System.Web;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
