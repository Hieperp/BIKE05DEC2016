using System.Text;
using RequireJsNet;

using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.PurchaseTasks;

using MVCDTO.PurchaseTasks;

using MVCClient.ViewModels.PurchaseTasks;
using MVCClient.Builders.PurchaseTasks;


namespace MVCClient.Controllers.PurchaseTasks
{
    public class PurchaseOrdersController : GenericSimpleController<PurchaseOrder, PurchaseOrderDTO, PurchaseOrderPrimitiveDTO, PurchaseOrderViewModel>
    {
        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService, IPurchaseOrderViewModelSelectListBuilder purchaseOrderViewModelSelectListBuilder)
            : base(purchaseOrderService, purchaseOrderViewModelSelectListBuilder)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Vehicles);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }
    }
}
