using MVCClient.ViewModels.SalesTasks;

namespace MVCClient.Builders.SalesTasks
{
    public interface IVehiclesInvoiceViewModelSelectListBuilder : IViewModelSelectListBuilder<VehiclesInvoiceViewModel>
    {
    }

    public interface IPartsInvoiceViewModelSelectListBuilder : IViewModelSelectListBuilder<PartsInvoiceViewModel>
    {
    }

    public interface IServicesInvoiceViewModelSelectListBuilder : IViewModelSelectListBuilder<ServicesInvoiceViewModel>
    {
    }
}