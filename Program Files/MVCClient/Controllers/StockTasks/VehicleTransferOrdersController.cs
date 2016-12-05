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
    public class VehicleTransferOrdersController : GenericViewDetailController<TransferOrder, TransferOrderDetail, VehicleTransferOrderViewDetail, VehicleTransferOrderDTO, VehicleTransferOrderPrimitiveDTO, VehicleTransferOrderDetailDTO, VehicleTransferOrderViewModel>
    {
        public VehicleTransferOrdersController(IVehicleTransferOrderService vehicleTransferOrderService, IVehicleTransferOrderViewModelSelectListBuilder vehicleTransferOrderViewModelSelectListBuilder)
            : base(vehicleTransferOrderService, vehicleTransferOrderViewModelSelectListBuilder)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Vehicles);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);

            RequireJsOptions.Add("FilterLocationID", -1, RequireJsOptionsScope.Page);
        }

    }


}