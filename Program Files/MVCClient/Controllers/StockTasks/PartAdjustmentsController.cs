using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.StockTasks;

using MVCDTO.StockTasks;

using MVCClient.ViewModels.StockTasks;
using MVCClient.Builders.StockTasks;



namespace MVCClient.Controllers.StockTasks
{
    public class PartAdjustmentsController : GenericViewDetailController<InventoryAdjustment, InventoryAdjustmentDetail, PartAdjustmentViewDetail, PartAdjustmentDTO, PartAdjustmentPrimitiveDTO, PartAdjustmentDetailDTO, PartAdjustmentViewModel>
    {
        public PartAdjustmentsController(IPartAdjustmentService partAdjustmentService, IPartAdjustmentViewModelSelectListBuilder partAdjustmentViewModelSelectListBuilder)
            : base(partAdjustmentService, partAdjustmentViewModelSelectListBuilder)
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
    }

}