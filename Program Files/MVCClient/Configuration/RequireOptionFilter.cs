using RequireJsNet;
using System.Web.Mvc;

using MVCBase.Enums;

namespace MVCClient.Configuration
{
    public class RequireOptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            RequireJsOptions.Add("rndQuantity", GlobalEnums.rndQuantity, RequireJsOptionsScope.Global);
            RequireJsOptions.Add("rndAmount", GlobalEnums.rndAmount , RequireJsOptionsScope.Global);
            RequireJsOptions.Add("rndDiscountPercent", GlobalEnums.rndDiscountPercent, RequireJsOptionsScope.Global);

            RequireJsOptions.Add("settingsManager.dateFormat", SettingsManager.DateFormat, RequireJsOptionsScope.Global);
        }
    }
}
