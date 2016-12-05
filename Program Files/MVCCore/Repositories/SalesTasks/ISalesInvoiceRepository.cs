using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCBase.Enums;


namespace MVCCore.Repositories.SalesTasks
{
    public interface ISalesInvoiceRepository : IGenericWithDetailRepository<SalesInvoice, SalesInvoiceDetail>
    {
    }

    public interface IVehiclesInvoiceRepository : ISalesInvoiceRepository
    {
    }

    public interface IPartsInvoiceRepository : ISalesInvoiceRepository
    {
    }

    public interface IServicesInvoiceRepository : ISalesInvoiceRepository
    {
        IList<ServiceInvoiceResult> GetActiveServiceInvoices(int locationID, int? serviceInvoiceID, string licensePlate, int isFinished);
        IList<RelatedPartsInvoiceValue> GetRelatedPartsInvoiceValue(int serviceInvoiceID);
    }


    public interface IVehiclesInvoiceAPIRepository : IGenericAPIRepository 
    {
    }

    public interface IPartsInvoiceAPIRepository : IGenericAPIRepository
    {
    }

    public interface IServicesInvoiceAPIRepository : IGenericAPIRepository
    {
        ICollection<ServicesInvoiceIndex> SearchServicesInvoiceIndexes(int serviceContractID);
    }
}
