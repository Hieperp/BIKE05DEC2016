using System.Text;
using System.Net;
using System.Web.Mvc;

using RequireJsNet;

using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Services.StockTasks;
using MVCClient.Builders.StockTasks;
using MVCClient.ViewModels.StockTasks;
using MVCDTO.StockTasks;


namespace MVCClient.Controllers.SalesTasks
{
    public class PartTransfersController : GenericViewDetailController<StockTransfer, StockTransferDetail, PartTransferViewDetail, PartTransferDTO, PartTransferPrimitiveDTO, PartTransferDetailDTO, PartTransferViewModel>
    {
        public PartTransfersController(IPartTransferService partTransferService, IPartTransferViewModelSelectListBuilder partTransferViewModelSelectListBuilder)
            : base(partTransferService, partTransferViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        public virtual ActionResult GetPendingPartTransferOrders()
        {
            return View();
        }


    }

}