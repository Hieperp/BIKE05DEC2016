using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.SalesTasks;

namespace MVCCore.Repositories.SalesTasks
{
    public interface IServiceContractRepository : IGenericRepository<ServiceContract>
    {
        IList<string> SearchAgentName(string agentName);
        IList<ServiceContractResult> SearchServiceContracts(string searchText);
        
        ICollection<ServiceContractGetVehiclesInvoice> ServiceContractGetVehiclesInvoice(int locationID, string searchText, int? salesInvoiceID, int? serviceContractID);
    }

    public interface IServiceContractAPIRepository : IGenericAPIRepository
    {
    }

}
