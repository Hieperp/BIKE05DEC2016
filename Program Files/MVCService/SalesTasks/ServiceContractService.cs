using MVCModel.Models;
using MVCDTO.SalesTasks;
using MVCCore.Repositories.SalesTasks;
using MVCCore.Services.SalesTasks;


namespace MVCService.SalesTasks
{
    public class ServiceContractService : GenericService<ServiceContract, ServiceContractDTO, ServiceContractPrimitiveDTO>, IServiceContractService
    {
        private readonly IServiceContractRepository serviceContractRepository;

        public ServiceContractService(IServiceContractRepository serviceContractRepository)
            : base(serviceContractRepository, "ServiceContractPostSaveValidate", "ServiceContractSaveRelative")
        {
            this.serviceContractRepository = serviceContractRepository;
        }

        public new bool Save(ServiceContractDTO dto, bool useExistingTransaction)
        {
            return base.Save(dto, true);
        }
    }
}
