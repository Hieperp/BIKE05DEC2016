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
                    invoices.Inv = new Inv() { key = (accountInvoiceSheets[0].AccountInvoiceID + 10).ToString() };

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


                    string xmlContent = ""; string responedMessage = "";
                    using (var stringwriter = new System.IO.StringWriter())
                    {
                        var serializer = new XmlSerializer(typeof(Invoices));
                        serializer.Serialize(stringwriter, invoices);
                        xmlContent = stringwriter.ToString();
                    }
                    xmlContent = xmlContent.Replace(">@#@<", "><"); xmlContent = xmlContent.Replace("\r", ""); xmlContent = xmlContent.Replace("\n", ""); xmlContent = xmlContent.Replace("\"", "'"); xmlContent = xmlContent.Replace("<?xml version='1.0' encoding='utf-16'?>", "");

                    xmlContent = "<x:Envelope xmlns:x='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/'> <x:Header /> <x:Body> <tem:ImportAndPublishInv> <tem:Account>tanthanh-cn1admin</tem:Account> <tem:ACpass>Einv@oi@vn#pt20</tem:ACpass> <tem:xmlInvData> <![CDATA[" + xmlContent;
                    xmlContent += "]]></tem:xmlInvData><tem:username>tanthanhcn1service</tem:username><tem:password>Einv@oi@vn#pt20</tem:password><tem:pattern>01GTKT0/001</tem:pattern><tem:serial>TT/20E</tem:serial><tem:convert>0</tem:convert></tem:ImportAndPublishInv></x:Body></x:Envelope>";


                    if (this.SOAPPost("https://tanthanh-cn1admindemo.vnpt-invoice.com.vn/PublishService.asmx", "http://tempuri.org/ImportAndPublishInv", xmlContent, out responedMessage))
                        return RedirectToAction("Index");
                    else
                        throw new System.ArgumentException("Lỗi xuất HĐ điện tử", responedMessage);
                }
                else
                    throw new System.ArgumentException("Lỗi xuất HĐ điện tử", "Không thể mở hóa đơn để xuất.");
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