using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVCClient.Controllers.SalesTasks;
using MVCCore.Services.SalesTasks;
using MVCCore.Repositories.SalesTasks;
using MVCClient.Builders.SalesTasks;

using MVCModel.Models;
using MVCBase.Enums;
using MVCCore.Repositories.CommonTasks;

namespace MVCClient.Controllers.Init3006
{
    public class DeletePartsInvoicesController : PartsInvoicesController
    {
        private readonly IPartsInvoiceService partsInvoiceService;
        private readonly IPartsInvoiceViewModelSelectListBuilder partsInvoiceViewModelSelectListBuilder;
        private readonly IPartsInvoiceRepository partsInvoiceRepository;

        private readonly ICommodityRepository commodityRepository;

        public DeletePartsInvoicesController(IPartsInvoiceService partsInvoiceService, IPartsInvoiceViewModelSelectListBuilder partsInvoiceViewModelSelectListBuilder, IPartsInvoiceRepository partsInvoiceRepository, ICommodityRepository commodityRepository)
            : base(partsInvoiceService, partsInvoiceViewModelSelectListBuilder)
        {
            this.partsInvoiceService = partsInvoiceService;
            this.partsInvoiceViewModelSelectListBuilder = partsInvoiceViewModelSelectListBuilder;
            this.partsInvoiceRepository = partsInvoiceRepository;

            this.commodityRepository = commodityRepository;
        }

        public ActionResult InitOfficialCode22DEC15()
        {
            ////////////////this.commodityRepository.InitOfficialCode22DEC15();
            return View();
        }
        // GET: DeletePartsInvoices
        public ActionResult DeletePartsInvoices()
        {
            //////////////List<SalesInvoice> salesInvoices = this.partsInvoiceRepository.GetEntities().Where(w => w.LocationID == this.partsInvoiceService.LocationID && w.SalesInvoiceTypeID == (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice && w.EntryDate.Year == 2015 && (w.EntryDate.Month == 7 || w.EntryDate.Month == 8 || w.EntryDate.Month == 9)).ToList();
            //////////////foreach (SalesInvoice salesInvoice in salesInvoices)
            //////////////{
            //////////////    this.partsInvoiceService.Delete(salesInvoice.SalesInvoiceID);
            //////////////}

            return View();
        }
    }
}