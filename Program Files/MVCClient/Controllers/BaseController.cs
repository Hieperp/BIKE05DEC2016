using RequireJsNet;

using MVCCore.Services;
using MVCClient.Api.SessionTasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text;

namespace MVCClient.Controllers
{
    public abstract class BaseController : CoreController
    {
        private readonly IBaseService baseService;
        public BaseController(IBaseService baseService)
        { this.baseService = baseService; }


        public IBaseService BaseService { get { return this.baseService; } }



        public virtual void AddRequireJsOptions()
        {
            int nmvnModuleID = this.baseService.NmvnModuleID;
            MenuSession.SetModuleID(this.HttpContext, nmvnModuleID);

            RequireJsOptions.Add("LocationID", this.baseService.LocationID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnModuleID", nmvnModuleID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnTaskID", this.baseService.NmvnTaskID, RequireJsOptionsScope.Page);
        }

        public virtual bool SOAPPost(string requestUri, string soapAction, string xmlContent, out string apiMessage)
        {
            var httpClient = new HttpClient();

            //var xmlContent = "<x:Envelope xmlns:x='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/'> <x:Header /> <x:Body> <tem:ImportAndPublishInv> <tem:Account>tanthanh-cn1admin</tem:Account> <tem:ACpass>Einv@oi@vn#pt20</tem:ACpass> <tem:xmlInvData> <![CDATA[<Invoices xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'> <Inv><key>V0000212</key> <Invoice> <CusCode>ABC001</CusCode> <CusName>NGUYỄN HOÀNG NHỰT</CusName> <CusAddress>ẤP TÂN AN, XÃ TÂN BÌNH, H.CHÂU THÀNH, ĐỒNG THÁP</CusAddress><CusPhone>0967485112</CusPhone> <CusTaxCode></CusTaxCode> <PaymentMethod> TM</PaymentMethod> <KindOfService>123</KindOfService> <Products> <Product> <ProdName>XE HONDA JC765 FUTURE FI (C)</ProdName> <ProdUnit>Chiếc</ProdUnit> <ProdQuantity>1</ProdQuantity> <ProdPrice>31727273</ProdPrice> <Amount>34900000</Amount> <Remark>My Remark</Remark> <Total>31727273</Total><VATRate>10</VATRate><VATAmount>3172727</VATAmount><Extra1>Mở rộng 1</Extra1><Extra2>Mở rộng 2</Extra2><Discount>0</Discount><DiscountAmount>0</DiscountAmount><IsSum>1</IsSum></Product></Products><Total>3181818</Total><DiscountRate>0</DiscountRate><DiscountAmount>0</DiscountAmount><VATRate>10</VATRate> <VATAmount>318182</VATAmount> <Amount>3500000</Amount><AmountInWords>Ba triệu năm trăm nghìn đồng chẵn</AmountInWords> <ConvertedAmount>0</ConvertedAmount> <Extra></Extra><Extra1>Extra1</Extra1><Extra2>Extra1</Extra2><ArisingDate>03/12/2020</ArisingDate><PaymentStatus>1</PaymentStatus><ResourceCode></ResourceCode><GrossValue></GrossValue><GrossValue0></GrossValue0><VatAmount0></VatAmount0><GrossValue5></GrossValue5><VatAmount5></VatAmount5><GrossValue10></GrossValue10><VatAmount10></VatAmount10></Invoice></Inv></Invoices>]]></tem:xmlInvData><tem:username>tanthanhcn1service</tem:username><tem:password>Einv@oi@vn#pt20</tem:password><tem:pattern>01GTKT0/001</tem:pattern><tem:serial>TT/20E</tem:serial><tem:convert>0</tem:convert></tem:ImportAndPublishInv></x:Body></x:Envelope>";

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri); //"https://tanthanh-cn1admindemo.vnpt-invoice.com.vn/PublishService.asmx"
            requestMessage.Headers.Add("SOAPAction", soapAction); //"http://tempuri.org/ImportAndPublishInv"
            requestMessage.Method = HttpMethod.Post; requestMessage.Content = new StringContent(xmlContent);
            requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");

            var sendRespone = httpClient.SendAsync(requestMessage); var sendResult = sendRespone.Result; sendResult.EnsureSuccessStatusCode();
            if (sendResult.IsSuccessStatusCode)
            {
                var receivedStream = sendResult.Content.ReadAsStreamAsync(); var receivedResult = receivedStream.Result;
                StreamReader readStream = new StreamReader(receivedResult, Encoding.UTF8);
                apiMessage = readStream.ReadToEnd();

                return true;
            }
            else { apiMessage = sendResult.ReasonPhrase; return false; }
        }
    }
}