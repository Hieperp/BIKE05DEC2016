﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using MVCCore.Repositories.Analysis;
using MVCModel.Models;
using RequireJsNet;
using MVCClient.Api.SessionTasks;
using System.Net;
using MVCClient.ViewModels.Helpers;
using MVCClient.Models;
using MVCCore.Helpers;

namespace MVCClient.Controllers.Analysis
{
    public class ReportsController : CoreController
    {
        private readonly IModuleRepository moduleRepository;

        private IReportRepository reportRepository;
        public ReportsController(IModuleRepository moduleRepository, IReportRepository reportRepository)
        {
            this.moduleRepository = moduleRepository;
            this.reportRepository = reportRepository;
        }

        public ActionResult Index()
        {
            MenuSession.SetModuleID(this.HttpContext, 9);                

            //RequireJsOptions.Add("LocationID", this.baseService.LocationID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnModuleID", 9, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnTaskID", 0, RequireJsOptionsScope.Page);

            return View(this.reportRepository.GetReports());
        }



        public ActionResult Open(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Report report = this.reportRepository.GetReports().Where(w => w.ReportUniqueID == id).FirstOrDefault();
            if (report == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            //BEGIN: Cho nay: sau nay can phai bo di, vi lam nhu the nay khong hay ho gi ca. Thay vao do, se thua ke tu base controller -> de lay userid, locationid, location official name
            var Db = new ApplicationDbContext();

            string aspUserID = User.Identity.GetUserId();
            int userID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;


            int locationID = this.moduleRepository.GetLocationID(userID);
            //BEGIN: Cho nay: sau nay can phai bo di, vi lam nhu the nay khong hay ho gi ca. Thay vao do, se thua ke tu base controller -> de lay userid, locationid, location official name



            PrintViewModel printViewModel = new PrintViewModel() { Id = locationID, ReportPath = report.ReportURL };

            return View(viewName: "Open", model: printViewModel);
        }



    }
}