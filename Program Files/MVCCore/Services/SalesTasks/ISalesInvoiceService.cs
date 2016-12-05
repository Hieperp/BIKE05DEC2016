using MVCModel.Models;
using MVCDTO.SalesTasks;
using MVCCore.Services.Helpers;

namespace MVCCore.Services.SalesTasks
{
    public interface IVehiclesInvoiceService : IGenericWithViewDetailService<SalesInvoice, SalesInvoiceDetail, VehiclesInvoiceViewDetail, VehiclesInvoiceDTO, VehiclesInvoicePrimitiveDTO, VehiclesInvoiceDetailDTO>
    {
    }

    public interface IPartsInvoiceService : IGenericWithViewDetailService<SalesInvoice, SalesInvoiceDetail, PartsInvoiceViewDetail, PartsInvoiceDTO, PartsInvoicePrimitiveDTO, PartsInvoiceDetailDTO>
    {
    }

    public interface IServicesInvoiceService : IGenericWithDetailService<SalesInvoice, SalesInvoiceDetail, ServicesInvoiceDTO, ServicesInvoicePrimitiveDTO, ServicesInvoiceDetailDTO>
    {
    }




    public interface IPartsInvoiceHelperService : IHelperService<SalesInvoice, SalesInvoiceDetail, PartsInvoiceDTO, PartsInvoiceDetailDTO>
    {
    }

}

