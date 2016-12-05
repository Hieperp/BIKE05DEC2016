using System.Text;
using System.Web.Mvc;

using RequireJsNet;

using MVCModel.Models;
using MVCBase.Enums;
using MVCCore.Services.StockTasks;
using MVCClient.Builders.StockTasks;
using MVCDTO.StockTasks;
using MVCClient.ViewModels.StockTasks;



namespace MVCClient.Controllers.SalesTasks
{
    public class VehicleTransfersController : GenericViewDetailController<StockTransfer, StockTransferDetail, VehicleTransferViewDetail, VehicleTransferDTO, VehicleTransferPrimitiveDTO, VehicleTransferDetailDTO, VehicleTransferViewModel>
    {
        public VehicleTransfersController(IVehicleTransferService vehicleTransferService, IVehicleTransferViewModelSelectListBuilder vehicleTransferViewModelSelectListBuilder)
            : base(vehicleTransferService, vehicleTransferViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Vehicles);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        public virtual ActionResult GetPendingVehicleTransferOrders()
        {
            return View();
        }
    }

}