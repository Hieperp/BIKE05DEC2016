using System.Net;
using System.Web.Mvc;

using MVCModel.Models;

using MVCCore.Services.SalesTasks;

using MVCDTO.SalesTasks;

using MVCClient.ViewModels.SalesTasks;
using MVCClient.Builders.SalesTasks;

namespace MVCClient.Controllers.SalesTasks
{
    public class ServiceContractsController : GenericSimpleController<ServiceContract, ServiceContractDTO, ServiceContractPrimitiveDTO, ServiceContractViewModel>
    {
        public ServiceContractsController(IServiceContractService serviceContractService, IServiceContractViewModelSelectListBuilder serviceContractViewModelSelectListBuilder)
            : base(serviceContractService, serviceContractViewModelSelectListBuilder, true)
        {
        }

    }
}