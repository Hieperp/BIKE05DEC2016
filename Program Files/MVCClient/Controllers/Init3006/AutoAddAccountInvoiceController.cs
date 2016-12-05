using System.Linq;
using System.Web.Mvc;
using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;

using MVCClient.Controllers.SalesTasks;
using System.Collections.Generic;
using MVCCore.Repositories.SalesTasks;
using MVCBase.Enums;
using System;

namespace MVCClient.Controllers.Init3006
{
    public class AutoAddAccountInvoiceController : AccountInvoicesController
    {
        private readonly IAccountInvoiceService accountInvoiceService;
        private readonly IVehiclesInvoiceRepository vehiclesInvoiceRepository;
        private readonly IAccountInvoiceAPIRepository accountInvoiceAPIRepository;

        public AutoAddAccountInvoiceController(IAccountInvoiceService accountInvoiceService, IVehiclesInvoiceRepository vehiclesInvoiceRepository, IAccountInvoiceAPIRepository accountInvoiceAPIRepository, IAccountInvoiceViewModelSelectListBuilder accountInvoiceViewModelSelectListBuilder)
            : base(accountInvoiceService, accountInvoiceViewModelSelectListBuilder)
        {
            this.accountInvoiceService = accountInvoiceService;
            this.vehiclesInvoiceRepository = vehiclesInvoiceRepository;
            this.accountInvoiceAPIRepository = accountInvoiceAPIRepository;
        }



////////////        UPDATE       SalesInvoices
////////////SET                VATInvoiceSeries = N'#'
////////////WHERE        (Approved = 1) AND (VATInvoiceSeries IS NULL)

////////////UPDATE       SalesInvoices
////////////SET                VATInvoiceDate = EntryDate
////////////WHERE        (Approved = 1) AND (VATInvoiceDate IS NULL)

////////////UPDATE       SalesInvoices
////////////SET                VATInvoiceNo = N'#'
////////////WHERE        (Approved = 1) AND (VATInvoiceDate IS NULL)


        //////Update SalesInvoices
        //////Set  Approved = 0
        //////Update SalesInvoices
        //////Set  Approved = 1
        //////WHERE        (SalesInvoiceID IN
        //////                             (SELECT        SalesInvoiceID
        //////                               FROM            SalesInvoiceDetails
        //////                               WHERE        (AccountInvoiceID IS NULL) AND (YEAR(EntryDate) = 2016) AND (SalesInvoiceTypeID = 10)))

        public ActionResult AddAccountInvoices()
        {
            List<SalesInvoice> salesInvoices = this.vehiclesInvoiceRepository.GetEntities().Where(w => w.LocationID == this.accountInvoiceService.LocationID && w.SalesInvoiceTypeID == (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice && w.Approved && w.EntryDate.Year == 2016).ToList();
            foreach (SalesInvoice salesInvoice in salesInvoices)
            {
                AccountInvoiceDTO accountInvoiceDTO = new AccountInvoiceDTO();

                accountInvoiceDTO.EntryDate = salesInvoice.EntryDate;
                accountInvoiceDTO.CustomerID = salesInvoice.CustomerID;

                accountInvoiceDTO.VATInvoiceNo = salesInvoice.VATInvoiceNo;
                accountInvoiceDTO.VATInvoiceSeries = salesInvoice.VATInvoiceSeries;
                accountInvoiceDTO.VATInvoiceDate = salesInvoice.VATInvoiceDate;

                accountInvoiceDTO.PreparedPersonID = salesInvoice.PreparedPersonID;
                accountInvoiceDTO.ApproverID = salesInvoice.ApproverID;

                accountInvoiceDTO.TotalQuantity = salesInvoice.TotalQuantity;
                accountInvoiceDTO.TotalAmount = salesInvoice.TotalAmount;
                accountInvoiceDTO.TotalVATAmount = salesInvoice.TotalVATAmount;
                accountInvoiceDTO.TotalGrossAmount = salesInvoice.TotalGrossAmount;
                accountInvoiceDTO.AverageDiscountPercent = salesInvoice.AverageDiscountPercent;

                accountInvoiceDTO.Description = salesInvoice.Description;
                accountInvoiceDTO.Remarks = salesInvoice.Remarks;

                IEnumerable<PendingSalesInvoice> pendingSalesInvoices = this.accountInvoiceAPIRepository.GetPendingSalesInvoices(salesInvoice.SalesInvoiceID, "", salesInvoice.LocationID, 0, DateTime.Now, DateTime.Now, 0, "");
                foreach (PendingSalesInvoice pendingSalesInvoice in pendingSalesInvoices)
                {
                    accountInvoiceDTO.ViewDetails.Add(new AccountInvoiceDetailDTO { SalesInvoiceDetailID = pendingSalesInvoice.SalesInvoiceDetailID, CommodityID = pendingSalesInvoice.CommodityID, CommodityTypeID = pendingSalesInvoice.CommodityTypeID, Quantity = pendingSalesInvoice.Quantity, ListedPrice = pendingSalesInvoice.ListedPrice, DiscountPercent = pendingSalesInvoice.DiscountPercent, UnitPrice = pendingSalesInvoice.UnitPrice, VATPercent = pendingSalesInvoice.VATPercent, GrossPrice = pendingSalesInvoice.GrossPrice, Amount = pendingSalesInvoice.Amount, VATAmount = pendingSalesInvoice.VATAmount, GrossAmount = pendingSalesInvoice.GrossAmount, IsBonus = pendingSalesInvoice.IsBonus, IsWarrantyClaim = pendingSalesInvoice.IsWarrantyClaim });
                }

                this.accountInvoiceService.UserID = salesInvoice.UserID; //THE BaseService.UserID IS AUTOMATICALLY SET BY CustomControllerAttribute OF CONTROLLER, ONLY WHEN BaseService IS INITIALIZED BY CONTROLLER. BUT HERE, THE this.accountInvoiceService IS INITIALIZED BY VehiclesInvoiceService => SO SHOULD SET accountInvoiceService.UserID = this.UserID
                this.accountInvoiceService.Save(accountInvoiceDTO);
            }

            return View();
        }

    }
}