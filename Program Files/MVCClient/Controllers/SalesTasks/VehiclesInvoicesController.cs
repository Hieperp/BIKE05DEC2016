﻿using System.Web.Mvc;

using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;
using MVCClient.ViewModels.Helpers;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace MVCClient.Controllers.SalesTasks
{
    public class VehiclesInvoicesController : GenericViewDetailController<SalesInvoice, SalesInvoiceDetail, VehiclesInvoiceViewDetail, VehiclesInvoiceDTO, VehiclesInvoicePrimitiveDTO, VehiclesInvoiceDetailDTO, VehiclesInvoiceViewModel>
    {
        public VehiclesInvoicesController(IVehiclesInvoiceService vehiclesInvoiceService, IVehiclesInvoiceViewModelSelectListBuilder vehiclesInvoiceViewModelSelectListBuilder)
            : base(vehiclesInvoiceService, vehiclesInvoiceViewModelSelectListBuilder)
        {
        }

        [OnResultExecutingFilterAttribute]
        public ActionResult PrintInvoice(int? id, int? pgid)
        {
            PrintViewModel printViewModel = InitPrintViewModel(id);
            printViewModel.PrintOptionID = (int)pgid;
            return View(printViewModel);
        }

        protected override PrintViewModel InitPrintViewModel(int? id)
        {
            PrintViewModel printViewModel = base.InitPrintViewModel(id);
            printViewModel.ViewName = "PrintInvoice";
            printViewModel.PrintOptionID = 1; //NOT IsFinished YET => PRINTED BY VehiclesInvoiceID

            SalesInvoice entity = base.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Readable);

            if (entity != null)
            {
                if (entity.IsFinished && entity.SalesInvoiceDetails.Count > 0)
                    foreach (SalesInvoiceDetail salesInvoiceDetail in entity.SalesInvoiceDetails)
                    {
                        if (salesInvoiceDetail.AccountInvoiceID != null) { printViewModel.Id = (int)salesInvoiceDetail.AccountInvoiceID; printViewModel.PrintOptionID = 0; }
                        break;
                    }
            }
            else
                printViewModel.Id = 0;

            return printViewModel;
        }
    }
}