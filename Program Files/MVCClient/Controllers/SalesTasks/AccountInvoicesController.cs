using System.Web.Mvc;
using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;
using MVCClient.ViewModels.Helpers;
using MVCBase.Enums;
using System.Net;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace MVCClient.Controllers.SalesTasks
{
    public class AccountInvoicesController : GenericViewDetailController<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO, AccountInvoiceViewModel>
    {
        private IAccountInvoiceService accountInvoiceService;
        public AccountInvoicesController(IAccountInvoiceService accountInvoiceService, IAccountInvoiceViewModelSelectListBuilder accountInvoiceViewModelSelectListBuilder)
            : base(accountInvoiceService, accountInvoiceViewModelSelectListBuilder)
        {
            this.accountInvoiceService = accountInvoiceService;
        }

        [OnResultExecutingFilterAttribute]
        public ActionResult PrintInvoice(int? id, int? pgid)
        {
            PrintViewModel printViewModel = InitPrintViewModel(id);
            printViewModel.PrintOptionID = (int)pgid;
            return View(printViewModel);
        }

        protected override ViewModels.Helpers.PrintViewModel InitPrintViewModel(int? id)
        {
            PrintViewModel printViewModel = base.InitPrintViewModel(id);
            printViewModel.ViewName = "PrintInvoice";
            return printViewModel;
        }

        public virtual ActionResult GetPendingSalesInvoices()
        {
            return View();
        }


        #region VNPT SOAP
        [AccessLevelAuthorize, ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult PublishInvoice(int? id)
        {
            return View(id);
        }


        [HttpPost, ActionName("PublishInvoice")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult PublishInvoiceConfirmed(int id)
        {
            try
            {
                AccountInvoice accountInvoice = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Editable);
                if (accountInvoice == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                List<AccountInvoiceSheet> accountInvoiceSheets = this.accountInvoiceService.GetAccountInvoiceSheet(id);
                if (accountInvoiceSheets.Count > 0)
                {

                    Invoices invoices = new Invoices();
                    invoices.Inv = new Inv() { key = accountInvoiceSheets[0].AccountInvoiceID.ToString() };

                    invoices.Inv.Invoice.ArisingDate = accountInvoiceSheets[0].VATInvoiceDate.ToString("dd/MM/yyyy");

                    invoices.Inv.Invoice.CusName = accountInvoiceSheets[0].CustomerName;
                    invoices.Inv.Invoice.CusTaxCode = accountInvoiceSheets[0].VATCode;
                    invoices.Inv.Invoice.CusPhone = accountInvoiceSheets[0].Telephone;
                    invoices.Inv.Invoice.CusAddress = accountInvoiceSheets[0].AddressNo + " " + accountInvoiceSheets[0].EntireTerritoryEntireName;
                    invoices.Inv.Invoice.PaymentMethod = accountInvoiceSheets[0].PaymentTermName;

                    invoices.Inv.Invoice.Total = accountInvoiceSheets[0].TotalAmount.ToString("0");
                    invoices.Inv.Invoice.VATRate = accountInvoiceSheets.Max(w => w.VATPercent).ToString("0");
                    invoices.Inv.Invoice.VATAmount = accountInvoiceSheets[0].TotalVATAmount.ToString("0");
                    invoices.Inv.Invoice.Amount = accountInvoiceSheets[0].TotalGrossAmount.ToString("0");
                    invoices.Inv.Invoice.AmountInWords = accountInvoiceSheets[0].TotalGrossAmountInWords;
                    
                    foreach (AccountInvoiceSheet accountInvoiceSheet in accountInvoiceSheets)
                    {
                        Product product = new Product();
                        product.ProdName = accountInvoiceSheet.Name + (accountInvoiceSheet.ColorCodeName != null ? "\\Màu: " + accountInvoiceSheet.ColorCodeName : "") + (accountInvoiceSheet.ChassisCode != null ? "\\SK: " + accountInvoiceSheet.ChassisCode : "") + (accountInvoiceSheet.EngineCode != null ? "\\SM: " + accountInvoiceSheet.EngineCode : "");
                        product.ProdUnit = accountInvoiceSheet.SalesUnit;
                        product.ProdQuantity = accountInvoiceSheet.Quantity.ToString("0");
                        product.ProdPrice = accountInvoiceSheet.UnitPrice.ToString("0");
                        product.Total = accountInvoiceSheet.Amount.ToString("0");

                        invoices.Inv.Invoice.Products.Add(product);
                    }


                    string xml = "";

                    using (var stringwriter = new System.IO.StringWriter())
                    {
                        var serializer = new XmlSerializer(typeof(Invoices));
                        serializer.Serialize(stringwriter, invoices);
                        xml = stringwriter.ToString();
                    }

                    xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf - 16\"?>", "");
                    xml.Replace("\\\"", "'");
                    xml.Replace("\r\n", "");
                    xml.Replace(">@#@<", "><");
                    

                    //XmlSerializer serializer = new XmlSerializer(typeof(Invoices));
                    //using (StringReader reader = new StringReader())
                    //{
                    //    var test = (Invoices)serializer.Deserialize(reader);
                    //}

                    //if (this.GenericService.PublishInvoice(id, inActive))
                    //    return RedirectToAction("Index");
                    //else
                    //    throw new System.ArgumentException("Lỗi vô hiệu dữ liệu", "Dữ liệu này không thể vô hiệu.");

                    return RedirectToAction("PublishInvoice", id);
                }
                else
                    return RedirectToAction("PublishInvoice", id);

            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("PublishInvoice", id);
            }
        }

        #endregion
    }
}