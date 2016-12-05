using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{
    public class ServiceContractTypeRepository : IServiceContractTypeRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public ServiceContractTypeRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<ServiceContractType> GetAllServiceContractTypes()
        {
            return this.totalBikePortalsEntities.ServiceContractTypes.ToList();
        }
    }
}
