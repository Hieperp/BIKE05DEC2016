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
        public virtual ActionResult PublishInvoice(int id)
        {
            PublishApi publishApi = new PublishApi() { AccountInvoiceID = id };
            return View(publishApi);
        }


        [HttpPost, ActionName("PublishInvoice")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult PublishInvoiceConfirmed(PublishApi publishApi)
        {
            try
            {
                AccountInvoice accountInvoice = this.GetEntityAndCheckAccessLevel(publishApi.AccountInvoiceID, GlobalEnums.AccessLevel.Editable);
                if (accountInvoice == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                List<AccountInvoiceSheet> accountInvoiceSheets = this.accountInvoiceService.GetAccountInvoiceSheet(accountInvoice.AccountInvoiceID).OrderBy(o => o.IsBonus).ThenBy(od => od.Code).ThenBy(ob => ob.Name).ToList(); //OrderBy IS VERY IMPORTANT FOR THE BELOW STATEMENTS
                if (accountInvoiceSheets.Count > 0 && accountInvoiceSheets[0].Approved)
                {

                    Invoices invoices = new Invoices();
                    invoices.Inv = new Inv() { key = (accountInvoiceSheets[0].AccountInvoiceID).ToString() + (accountInvoiceSheets[0].ApiPublishID == null ? "" : "#" + accountInvoiceSheets[0].ApiPublishID.ToString()) };

                    invoices.Inv.Invoice.ArisingDate = accountInvoiceSheets[0].VATInvoiceDate.ToString("dd/MM/yyyy");

                    if (string.IsNullOrEmpty(accountInvoiceSheets[0].VATCode) || accountInvoiceSheets[0].VATCode.Trim() == "") invoices.Inv.Invoice.Buyer = accountInvoiceSheets[0].CustomerName; else invoices.Inv.Invoice.CusName = accountInvoiceSheets[0].CustomerName;

                    invoices.Inv.Invoice.CusTaxCode = (!string.IsNullOrEmpty(accountInvoiceSheets[0].VATCode) && accountInvoiceSheets[0].VATCode.Trim() == "-")? "" : accountInvoiceSheets[0].VATCode;
                    invoices.Inv.Invoice.CusPhone = accountInvoiceSheets[0].Telephone;
                    invoices.Inv.Invoice.CusAddress = accountInvoiceSheets[0].AddressNo + " " + accountInvoiceSheets[0].EntireTerritoryEntireName;
                    invoices.Inv.Invoice.PaymentMethod = accountInvoiceSheets[0].PaymentTermName;

                    invoices.Inv.Invoice.Total = accountInvoiceSheets[0].TotalAmount.ToString("0");
                    invoices.Inv.Invoice.VATRate = accountInvoiceSheets[0].IsBonus ? "-1" : accountInvoiceSheets.Max(w => w.VATPercent).ToString("0");
                    invoices.Inv.Invoice.VATAmount = accountInvoiceSheets[0].TotalVATAmount.ToString("0");
                    invoices.Inv.Invoice.Amount = accountInvoiceSheets[0].TotalGrossAmount.ToString("0");
                    invoices.Inv.Invoice.AmountInWords = accountInvoiceSheets[0].TotalGrossAmountInWords;

                    bool isBonus = false; int li = 0;

                    foreach (AccountInvoiceSheet accountInvoiceSheet in accountInvoiceSheets)
                    {
                        if (isBonus != accountInvoiceSheet.IsBonus)
                        {
                            isBonus = accountInvoiceSheet.IsBonus;
                            Product productA = new Product() { ProdName = "Hàng khuyến mãi không thu tiền:" };
                            invoices.Inv.Invoice.Products.Add(productA);
                        }

                        Product product = new Product();

                        li++; product.Extra1 = li.ToString("0");

                        product.ProdName = accountInvoiceSheet.Name + (accountInvoiceSheet.ColorCodeName != null ? "\\Màu: " + accountInvoiceSheet.ColorCodeName : "") + (accountInvoiceSheet.ChassisCode != null ? "\\SK: " + accountInvoiceSheet.ChassisCode : "") + (accountInvoiceSheet.EngineCode != null ? "\\SM: " + accountInvoiceSheet.EngineCode : "");
                        product.ProdUnit = accountInvoiceSheet.SalesUnit;
                        product.ProdQuantity = accountInvoiceSheet.Quantity.ToString("0");
                        product.ProdPrice = accountInvoiceSheet.UnitPrice.ToString("0");
                        product.Total = accountInvoiceSheet.Amount.ToString("0");

                        invoices.Inv.Invoice.Products.Add(product);
                    }


                    string xmlContent = ""; string apiMessage = "";
                    using (var stringwriter = new System.IO.StringWriter())
                    {
                        var serializer = new XmlSerializer(typeof(Invoices));
                        serializer.Serialize(stringwriter, invoices);
                        xmlContent = stringwriter.ToString();
                    }
                    xmlContent = xmlContent.Replace(">@#@<", "><"); xmlContent = xmlContent.Replace("\r", ""); xmlContent = xmlContent.Replace("\n", ""); xmlContent = xmlContent.Replace("<?xml version='1.0' encoding='utf-16'?>", "");

                    #region MyRegion
                    string apiAccount = publishApi.ApiAccount;// accountInvoiceSheets[0].ApiAccount;// "tanthanh-cn1admin";
                    string apiACPass = publishApi.ApiACPass; // accountInvoiceSheets[0].ApiACPass;// "Einv@oi@vn#pt20";
                    string apiUserName = accountInvoiceSheets[0].ApiUsername;// "tanthanhcn1service";
                    string apiPassWord = accountInvoiceSheets[0].ApiPass;// "Einv@oi@vn#pt20";
                    string invoicePattern = accountInvoiceSheets[0].ApiPattern;// "01GTKT0/001";
                    string invoiceSerial = accountInvoiceSheets[0].ApiSerial;// "TT/20E";
                    #endregion
                    string xmlData = xmlContent;
                    xmlContent = "<x:Envelope xmlns:x='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/'>";
                    xmlContent += "<x:Header /> <x:Body> <tem:ImportAndPublishInv> <tem:Account>" + apiAccount + "</tem:Account> <tem:ACpass>" + apiACPass + "</tem:ACpass> <tem:xmlInvData>";
                    xmlContent += "<![CDATA[" + xmlData;
                    xmlContent += "]]></tem:xmlInvData><tem:username>" + apiUserName + "</tem:username><tem:password>" + apiPassWord + "</tem:password>";
                    xmlContent += "<tem:pattern>" + invoicePattern + "</tem:pattern><tem:serial>" + invoiceSerial + "</tem:serial><tem:convert>0</tem:convert></tem:ImportAndPublishInv></x:Body></x:Envelope>";


                    if (this.SOAPPost("https://" + accountInvoiceSheets[0].ApiURL + "/PublishService.asmx", "http://tempuri.org/ImportAndPublishInv", xmlContent, out apiMessage))
                    {
                        //<ImportAndPublishInvResult>OK:01GTKT0/001;TT/20E-220564_455</ImportAndPublishInvResult>
                        int i = apiMessage.IndexOf("<ImportAndPublishInvResult>OK"); string apiSerialString = null; int apiSerialID = 0;
                        if (i != -1)
                        {
                            apiMessage = apiMessage.Substring(i + "<ImportAndPublishInvResult>".Length);
                            i = apiMessage.IndexOf("</ImportAndPublishInvResult>");
                            if (i != -1)
                            {
                                apiMessage = apiMessage.Substring(0, i);
                                i = apiMessage.IndexOf(invoiceSerial + "-" + invoices.Inv.key + "_");
                                if (i != -1)
                                {
                                    apiSerialString = apiMessage.Substring(i + (invoiceSerial + "-" + invoices.Inv.key + "_").Length);
                                    int.TryParse(apiSerialString, out apiSerialID);
                                }
                            }

                            this.accountInvoiceService.UpdateAccountInvoiceApi(accountInvoice.AccountInvoiceID, invoiceSerial, publishApi.ApiAccount, apiSerialID, apiSerialString, apiMessage);

                            return View("PublishSucceed", accountInvoice);
                        }
                        else
                        {
                            i = apiMessage.IndexOf("<ImportAndPublishInvResult>");
                            if (i != -1)
                            {
                                apiMessage = apiMessage.Substring(i + "<ImportAndPublishInvResult>".Length);
                                i = apiMessage.IndexOf("</ImportAndPublishInvResult>");
                                if (i != -1) apiMessage = apiMessage.Substring(0, i);
                            }
                            throw new System.ArgumentException("Lỗi xuất HĐ điện tử", apiMessage);
                        }
                    }
                    else
                        throw new System.ArgumentException("Lỗi xuất HĐ điện tử", apiMessage);
                }
                else
                    throw new System.ArgumentException("Lỗi xuất HĐ điện tử", "Không thể mở hóa đơn để xuất.");
            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("PublishInvoice", publishApi.AccountInvoiceID);
            }
        }
        #endregion ImportAndPublishInv



        #region ImportAndPublishInv
        [AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable), ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult ClearInvoice(int id)
        {
            PublishApi publishApi = new PublishApi() { AccountInvoiceID = id };
            return View(publishApi);
        }


        [HttpPost, ActionName("ClearInvoice")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult ClearInvoiceConfirmed(PublishApi publishApi)
        {
            try
            {
                AccountInvoice accountInvoice = this.GetEntityAndCheckAccessLevel(publishApi.AccountInvoiceID, GlobalEnums.AccessLevel.Readable);
                if (accountInvoice == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                AccountInvoiceViewModel accountInvoiceViewModel = this.GetViewModel(accountInvoice, true);

                if (accountInvoiceViewModel.Approved && this.GenericService.GetUnApprovalPermitted(accountInvoice.OrganizationalUnitID))
                { //NEED GetUnApprovalPermitted IN ORDER TO CLEAR API INVOICE
                    string fkey = "";

                    List<AccountInvoiceSheet> accountInvoiceSheets = this.accountInvoiceService.GetAccountInvoiceSheet(accountInvoice.AccountInvoiceID);
                    if (accountInvoiceSheets.Count > 0 && accountInvoiceSheets[0].Approved) { fkey = (accountInvoiceSheets[0].AccountInvoiceID).ToString() + (accountInvoiceSheets[0].ApiPublishID == null ? "" : "#" + accountInvoiceSheets[0].ApiPublishID.ToString()); } else throw new System.ArgumentException("Lỗi xuất HĐ điện tử", "Không thể mở hóa đơn để xóa.");

                    string xmlContent = ""; string apiMessage = "";
                    xmlContent = xmlContent.Replace(">@#@<", "><"); xmlContent = xmlContent.Replace("\r", ""); xmlContent = xmlContent.Replace("\n", ""); xmlContent = xmlContent.Replace("\"", "'"); xmlContent = xmlContent.Replace("<?xml version='1.0' encoding='utf-16'?>", "");

                    #region MyRegion
                    string apiAccount = publishApi.ApiAccount;// accountInvoiceSheets[0].ApiAccount;// "tanthanh-cn1admin";
                    string apiACPass = publishApi.ApiACPass; // accountInvoiceSheets[0].ApiACPass;// "Einv@oi@vn#pt20";
                    string apiUserName = accountInvoiceSheets[0].ApiUsername;// "tanthanhcn1service";
                    string apiPassWord = accountInvoiceSheets[0].ApiPass;// "Einv@oi@vn#pt20";
                    #endregion
                    string xmlData = xmlContent;
                    xmlContent = "<x:Envelope xmlns:x='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/'>";
                    xmlContent += "<x:Header /> <x:Body> <tem:cancelInv> <tem:Account>" + apiAccount + "</tem:Account> <tem:ACpass>" + apiACPass + "</tem:ACpass>";
                    xmlContent += "<tem:fkey>" + fkey + "</tem:fkey>";
                    xmlContent += "<tem:userName>" + apiUserName + "</tem:userName><tem:userPass>" + apiPassWord + "</tem:userPass>";
                    xmlContent += "</tem:cancelInv></x:Body></x:Envelope>";


                    if (this.SOAPPost("https://" + accountInvoiceSheets[0].ApiURL + "/BusinessService.asmx", "http://tempuri.org/cancelInv", xmlContent, out apiMessage))
                    {
                        //<cancelInvResult>OK:</cancelInvResult>
                        int i = apiMessage.IndexOf("<cancelInvResult>OK");
                        if (i != -1)
                        {
                            apiMessage = apiMessage.Substring(i + "<cancelInvResult>".Length);
                            i = apiMessage.IndexOf("</cancelInvResult>");
                            if (i != -1) apiMessage = apiMessage.Substring(0, i);

                            this.accountInvoiceService.ClearAccountInvoiceApi(accountInvoice.AccountInvoiceID, publishApi.ApiAccount);

                            return View("PublishSucceed", accountInvoice);
                        }
                        else
                        {
                            i = apiMessage.IndexOf("<cancelInvResult>");
                            if (i != -1)
                            {
                                apiMessage = apiMessage.Substring(i + "<cancelInvResult>".Length);
                                i = apiMessage.IndexOf("</cancelInvResult>");
                                if (i != -1) apiMessage = apiMessage.Substring(0, i);
                            }
                            throw new System.ArgumentException("Lỗi xóa HĐ điện tử", apiMessage);
                        }
                    }
                    else
                        throw new System.ArgumentException("Lỗi xóa HĐ điện tử", apiMessage);
                }
                else
                    throw new System.ArgumentException("Lỗi xóa HĐ điện tử", "Bạn không có quyền xóa hóa đơn này.");
            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("ClearInvoice", publishApi.AccountInvoiceID);
            }
        }
        #endregion ImportAndPublishInv
        #endregion

    }
}