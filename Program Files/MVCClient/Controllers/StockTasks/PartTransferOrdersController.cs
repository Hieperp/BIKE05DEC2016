using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using MVCModel.Models;
using MVCBase.Enums;
using MVCCore.Services.StockTasks;
using MVCClient.Builders.StockTasks;
using MVCClient.ViewModels.StockTasks;
using MVCDTO.StockTasks;


namespace MVCClient.Controllers.SalesTasks
{
    public class PartTransferOrdersController : GenericViewDetailController<TransferOrder, TransferOrderDetail, PartTransferOrderViewDetail, PartTransferOrderDTO, PartTransferOrderPrimitiveDTO, PartTransferOrderDetailDTO, PartTransferOrderViewModel>
    {
        public PartTransferOrdersController(IPartTransferOrderService partTransferOrderService, IPartTransferOrderViewModelSelectListBuilder partTransferOrderViewModelSelectListBuilder)
            : base(partTransferOrderService, partTransferOrderViewModelSelectListBuilder)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);

            RequireJsOptions.Add("FilterLocationID", this.BaseService.LocationID, RequireJsOptionsScope.Page);
        }

    }
}