using System.Web;
using System.Web.Mvc;

using MVCClient.Configuration;

namespace MVCClient
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            //filters.Add(new RequireHttpsAttribute());

            filters.Add(new RequireOptionFilter());
        }
    }
}
