using MVCBase.Enums;

using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;
using MVCClient.ViewModels.Helpers;



namespace MVCClient.Controllers.SalesTasks
{
    public class VehiclesInvoicesController : GenericViewDetailController<SalesInvoice, SalesInvoiceDetail, VehiclesInvoiceViewDetail, VehiclesInvoiceDTO, VehiclesInvoicePrimitiveDTO, VehiclesInvoiceDetailDTO, VehiclesInvoiceViewModel>
    {
        public VehiclesInvoicesController(IVehiclesInvoiceService vehiclesInvoiceService, IVehiclesInvoiceViewModelSelectListBuilder vehiclesInvoiceViewModelSelectListBuilder)
            : base(vehiclesInvoiceService, vehiclesInvoiceViewModelSelectListBuilder)
        {
        }

        protected override ViewModels.Helpers.PrintViewModel InitPrintViewModel(int? id)
        {
            PrintViewModel printViewModel = base.InitPrintViewModel(id);
            printViewModel.PrintOptionID = 1; //NOT IsFinished YET => PRINTED BY VehiclesInvoiceID

            SalesInvoice entity = base.GetEntityAndCheckAccessLevel(id, GlobalEnums.AccessLevel.Readable);

            if (entity != null)
            {
                if (entity.IsFinished && entity.SalesInvoiceDetails.Count > 0)
                    foreach (SalesInvoiceDetail salesInvoiceDetail in entity.SalesInvoiceDetails)
                    {
                        if (salesInvoiceDetail.AccountInvoiceID != null) { printViewModel.Id = (int)salesInvoiceDetail.AccountInvoiceID; printViewModel.PrintOptionID = 0; }
                        break;
                    }
            }
            else
                printViewModel.Id = 0;

            return printViewModel;
        }

    }
}