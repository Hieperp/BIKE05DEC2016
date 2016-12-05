using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using MVCClient.Models;
using MVCClient.ViewModels.Home;
using MVCClient.Api.SessionTasks;
using MVCModel.Models;
using MVCCore.Repositories;
using System.Data.Entity.Core.Objects;

namespace MVCClient.Controllers
{
    public class HomeController : CoreController
    {
        private readonly IBaseRepository baseRepository;
        public HomeController(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserGuide()
        {
            if (User.Identity.IsAuthenticated)
            {
                string aspUserID = User.Identity.GetUserId();

                var Db = new ApplicationDbContext();

                var userID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Options()
        {
            OptionViewModel optionViewModel = new OptionViewModel { GlobalFromDate = HomeSession.GetGlobalFromDate(this.HttpContext), GlobalToDate = HomeSession.GetGlobalToDate(this.HttpContext) };

            return View(optionViewModel);
        }

        [HttpPost]
        public ActionResult Options(OptionViewModel optionViewModel)
        {
            HomeSession.SetGlobalFromDate(this.HttpContext, optionViewModel.GlobalFromDate);
            HomeSession.SetGlobalToDate(this.HttpContext, optionViewModel.GlobalToDate);

            return View("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult LockedDate()
        {
            List<Location> Locations = this.baseRepository.GetEntities<Location>().ToList();

            return View(Locations);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult LockedDate(int locationID, DateTime lockedDate)
        {
            int x = locationID;
            DateTime d = lockedDate;
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("AspUserID", User.Identity.GetUserId()), new ObjectParameter("LocationID", locationID), new ObjectParameter("LockedDate", lockedDate) };
            this.baseRepository.ExecuteFunction("UpdateLockedDate", parameters);

            return Json(new { Success = true });
        }

    }
}