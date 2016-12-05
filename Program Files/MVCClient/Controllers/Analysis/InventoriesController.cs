using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Controllers.Analysis
{
    //[Authorize(Roles = "Admin")]
    public class InventoriesController : CoreController
    {
        public ActionResult WarehouseJournal()
        {
            return View();
        }

        public ActionResult WarehouseCard()
        {
            return View();
        }

        public ActionResult VehicleJournal()
        {
            return View();
        }

        public ActionResult VehicleCard()
        {
            return View();
        }

        public ActionResult SalesInvoiceJournal()
        {
            return View();
        }

        public ActionResult AccountInvoiceJournal()
        {
            return View();
        }

    }
}