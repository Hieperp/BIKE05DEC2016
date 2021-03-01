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

        public override ActionResult RedirectAfterApprove(AccountInvoiceViewModel simpleViewModel)
        {
            return RedirectToAction("Edit", new { id = simpleViewModel.GetID() });
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
        #region ImportAndPublishInv
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

                List<AccountInvoiceSheet> accountInvoiceSheets = this.accountInvoiceService.GetAccountInvoiceSheet(accountInvoice.AccountInvoiceID);
                if (accountInvoiceSheets.Count > 0 && accountInvoiceSheets[0].Approved)
                {

                    Invoices invoices = new Invoices();
                    invoices.Inv = new Inv() { key = (accountInvoiceSheets[0].AccountInvoiceID).ToString() };

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

                    #region MyRegion
                    string apiAccount = "tanthanh-cn1admin";
                    string apiACPass = "Einv@oi@vn#pt20";
                    string apiUserName = "tanthanhcn1service";
                    string apiPassWord = "Einv@oi@vn#pt20";
                    string invoicePattern = "01GTKT0/001";
                    string invoiceSerial = "TT/20E";
                    #endregion
                    string xmlData = xmlContent;
                    xmlContent = "<x:Envelope xmlns:x='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/'>";
                    xmlContent += "<x:Header /> <x:Body> <tem:ImportAndPublishInv> <tem:Account>" + apiAccount + "</tem:Account> <tem:ACpass>" + apiACPass + "</tem:ACpass> <tem:xmlInvData>";
                    xmlContent += "<![CDATA[" + xmlData;
                    xmlContent += "]]></tem:xmlInvData><tem:username>" + apiUserName + "</tem:username><tem:password>" + apiPassWord + "</tem:password>";
                    xmlContent += "<tem:pattern>" + invoicePattern + "</tem:pattern><tem:serial>" + invoiceSerial + "</tem:serial><tem:convert>0</tem:convert></tem:ImportAndPublishInv></x:Body></x:Envelope>";


                    if (this.SOAPPost("https://tanthanh-cn1admindemo.vnpt-invoice.com.vn/PublishService.asmx", "http://tempuri.org/ImportAndPublishInv", xmlContent, out responedMessage))
                    {
                        int i = responedMessage.IndexOf("<ImportAndPublishInvResult>OK"); string apiSerialString = null; int apiSerialID = 0;
                        if (i != -1)
                        {
                            responedMessage = responedMessage.Substring(i + "<ImportAndPublishInvResult>".Length);
                            i = responedMessage.IndexOf("</ImportAndPublishInvResult>");
                            if (i != -1)
                            {
                                responedMessage = responedMessage.Substring(0, i);
                                i = responedMessage.IndexOf(invoiceSerial + "-" + invoices.Inv.key + "_");
                                if (i != -1)
                                {
                                    apiSerialString = responedMessage.Substring(i + (invoiceSerial + "-" + invoices.Inv.key + "_").Length);
                                    int.TryParse(apiSerialString, out apiSerialID);
                                }
                            }

                            this.accountInvoiceService.UpdateAccountInvoiceApi(accountInvoice.AccountInvoiceID, apiSerialID, apiSerialString, responedMessage);

                            return View("PublishSucceed", accountInvoice);
                        }
                        else
                        {
                            i = responedMessage.IndexOf("<ImportAndPublishInvResult>ERR");
                            if (i != -1)
                            {
                                responedMessage = responedMessage.Substring(i + "<ImportAndPublishInvResult>".Length);
                                i = responedMessage.IndexOf("</ImportAndPublishInvResult>");
                                if (i != -1) responedMessage = responedMessage.Substring(0, i);
                            }
                            throw new System.ArgumentException("Lỗi xuất HĐ điện tử", responedMessage);
                        }
                    }
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
        #endregion ImportAndPublishInv



        #region ImportAndPublishInv
        [AccessLevelAuthorize, ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult ClearInvoice(int? id)
        {
            return View(id);
        }


        [HttpPost, ActionName("ClearInvoice")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult ClearInvoiceConfirmed(int id)
        {
            try
            {
                AccountInvoice accountInvoice = this.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Editable);
                if (accountInvoice == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                AccountInvoiceViewModel accountInvoiceViewModel = this.GetViewModel(accountInvoice, true);

                if (accountInvoiceViewModel.Approved && this.GenericService.GetUnApprovalPermitted(accountInvoice.OrganizationalUnitID) && this.GenericService.UnApprovable(accountInvoiceViewModel))
                {
                    string xmlContent = ""; string responedMessage = "";
                    xmlContent = xmlContent.Replace(">@#@<", "><"); xmlContent = xmlContent.Replace("\r", ""); xmlContent = xmlContent.Replace("\n", ""); xmlContent = xmlContent.Replace("\"", "'"); xmlContent = xmlContent.Replace("<?xml version='1.0' encoding='utf-16'?>", "");

                    #region MyRegion
                    string apiAccount = "tanthanh-cn1admin";
                    string apiACPass = "Einv@oi@vn#pt20";
                    string apiUserName = "tanthanhcn1service";
                    string apiPassWord = "Einv@oi@vn#pt20";
                    string invoiceSerial = "TT/20E";
                    string fkey = accountInvoiceViewModel.AccountInvoiceID.ToString();
                    #endregion
                    string xmlData = xmlContent;
                    xmlContent = "<x:Envelope xmlns:x='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/'>";
                    xmlContent += "<x:Header /> <x:Body> <tem:cancelInv> <tem:Account>" + apiAccount + "</tem:Account> <tem:ACpass>" + apiACPass + "</tem:ACpass> <tem:xmlInvData>";
                    xmlContent += "<tem:fkey>" + fkey + "</tem:fkey>";
                    xmlContent += "</tem:xmlInvData><tem:username>" + apiUserName + "</tem:username><tem:userPass>" + apiPassWord + "</tem:userPass>";
                    xmlContent += "</tem:cancelInv></x:Body></x:Envelope>";

                    //<cancelInvResult>OK:</cancelInvResult>
                    if (this.SOAPPost("https://tanthanh-cn1admindemo.vnpt-invoice.com.vn/BusinessService.asmx", "http://tempuri.org/cancelInv", xmlContent, out responedMessage))
                    {
                        int i = responedMessage.IndexOf("<CancelInvResult>OK"); string apiSerialString = null; int apiSerialID = 0;
                        if (i != -1)
                        {
                            responedMessage = responedMessage.Substring(i + "<CancelInvResult>".Length);
                            i = responedMessage.IndexOf("</CancelInvResult>");
                            if (i != -1)
                            {
                                responedMessage = responedMessage.Substring(0, i);
                                i = responedMessage.IndexOf(invoiceSerial + "-" + fkey + "_");
                                if (i != -1)
                                {
                                    apiSerialString = responedMessage.Substring(i + (invoiceSerial + "-" + fkey + "_").Length);
                                    int.TryParse(apiSerialString, out apiSerialID);
                                }
                            }

                            this.accountInvoiceService.UpdateAccountInvoiceApi(accountInvoice.AccountInvoiceID, apiSerialID, apiSerialString, responedMessage);

                            return View("PublishSucceed", accountInvoice);
                        }
                        else
                        {
                            i = responedMessage.IndexOf("<CancelInvResult>ERR");
                            if (i != -1)
                            {
                                responedMessage = responedMessage.Substring(i + "<CancelInvResult>".Length);
                                i = responedMessage.IndexOf("</CancelInvResult>");
                                if (i != -1) responedMessage = responedMessage.Substring(0, i);
                            }
                            throw new System.ArgumentException("Lỗi xóa HĐ điện tử", responedMessage);
                        }
                    }
                    else
                        throw new System.ArgumentException("Lỗi xóa HĐ điện tử", responedMessage);
                }
                else
                    throw new System.ArgumentException("Lỗi xóa HĐ điện tử", "Không thể mở hóa đơn để xóa.");
            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("ClearInvoice", id);
            }
        }
        #endregion ImportAndPublishInv
        #endregion

    }
}