using MVCModel.Models;
using MVCDTO.SalesTasks;

namespace MVCCore.Services.SalesTasks
{
    public interface IServiceContractService : IGenericService<ServiceContract, ServiceContractDTO, ServiceContractPrimitiveDTO>
    {
        bool Save(ServiceContractDTO dto, bool useExistingTransaction);
    }
}
