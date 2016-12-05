using System.Web.Mvc;
using System.Text;

using MVCBase.Enums;
using MVCClient.Configuration;

namespace MVCClient.Controllers
{
    public class CoreController : Controller
    {
        public ActionResult GlobalJavaScriptEnums()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("var SubmitTypeOption = " + typeof(GlobalEnums.SubmitTypeOption).EnumToJson() + "; ");
            stringBuilder.Append("var SettingsManager = " + System.Web.Helpers.Json.Encode(new MySettingsManager()) + "; ");

            return JavaScript(stringBuilder.ToString());
        }
    }
}