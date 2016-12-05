using System.Net;
using System.Web.Mvc;

using System.Text;
using RequireJsNet;

using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;


namespace MVCClient.Controllers.SalesTasks
{
    public class QuotationsController : GenericViewDetailController<Quotation, QuotationDetail, QuotationViewDetail, QuotationDTO, QuotationPrimitiveDTO, QuotationDetailDTO, QuotationViewModel>
    {
        public QuotationsController(IQuotationService quotationService, IQuotationViewModelSelectListBuilder quotationViewModelSelectListBuilder)
            : base(quotationService, quotationViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Services);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        
    }
}