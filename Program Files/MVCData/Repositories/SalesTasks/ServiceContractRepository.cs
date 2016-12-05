using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel.Models;
using MVCCore.Repositories.SalesTasks;

namespace MVCData.Repositories.SalesTasks
{
    public class ServiceContractRepository : GenericRepository<ServiceContract>, IServiceContractRepository
    {
        public ServiceContractRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, null, null, "ServicesContractDeletable")
        {
        }

        public IList<ServiceContractResult> SearchServiceContracts(string searchText)
        {
            return this.TotalBikePortalsEntities.SearchServiceContracts(searchText).ToList();
        }

        public IList<string> SearchAgentName(string agentName)
        {
            return this.TotalBikePortalsEntities.ServiceContracts.Where(w => w.AgentName.Contains(agentName)).Select(a => a.AgentName).Distinct().ToList();
        }



        public ICollection<ServiceContractGetVehiclesInvoice> ServiceContractGetVehiclesInvoice(int locationID, string searchText, int? salesInvoiceID, int? serviceContractID)
        {
            return this.TotalBikePortalsEntities.ServiceContractGetVehiclesInvoices(locationID, searchText, salesInvoiceID, serviceContractID).ToList();
        }
    }

    public class ServiceContractAPIRepository : GenericAPIRepository, IServiceContractAPIRepository
    {
        public ServiceContractAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetServiceContractIndexes")
        {
        }
    }

}
