using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Api
{
    public class GenericApiController : Controller
    {
        // GET: GenericApi
        public ActionResult Index()
        {
            return View();
        }
    }
}