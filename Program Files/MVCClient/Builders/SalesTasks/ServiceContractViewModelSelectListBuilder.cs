using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.SalesTasks;

namespace MVCClient.Builders.SalesTasks
{
    public class ServiceContractViewModelSelectListBuilder : IServiceContractViewModelSelectListBuilder
    {
        private readonly IServiceContractTypeRepository serviceContractTypeRepository;
        private readonly IServiceContractTypeSelectListBuilder serviceContractTypeSelectListBuilder;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;
        private readonly IAspNetUserRepository aspNetUserRepository;

        public ServiceContractViewModelSelectListBuilder(IServiceContractTypeSelectListBuilder serviceContractTypeSelectListBuilder,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IServiceContractTypeRepository serviceContractTypeRepository,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.serviceContractTypeRepository = serviceContractTypeRepository;
            this.serviceContractTypeSelectListBuilder = serviceContractTypeSelectListBuilder;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
            this.aspNetUserRepository = aspNetUserRepository;
        }

        public void BuildSelectLists(ServiceContractViewModel serviceContractViewModel)
        {
            serviceContractViewModel.ServiceContractTypeDropDown = serviceContractTypeSelectListBuilder.BuildSelectListItemsForServiceContractTypes(serviceContractTypeRepository.GetAllServiceContractTypes());
            serviceContractViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), serviceContractViewModel.UserID);
            serviceContractViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), serviceContractViewModel.UserID);
        }
        
    }
}