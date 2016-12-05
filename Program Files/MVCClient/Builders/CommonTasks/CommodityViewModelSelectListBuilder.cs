using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.CommonTasks;

namespace MVCClient.Builders.CommonTasks
{
    public class CommodityViewModelSelectListBuilder : ICommodityViewModelSelectListBuilder
    {
        private readonly ICommodityCategoryRepository commodityCategoryRepository;
        private readonly ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder;
        private readonly ICommodityTypeRepository commodityTypeRepository;
        private readonly ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder;
        private readonly ICustomerRepository customerRepository;
        private readonly ICustomerViewModelSelectListBuilder customerViewModelSelectListBuilder;

        public CommodityViewModelSelectListBuilder(ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder,
                                    ICommodityCategoryRepository commodityCategoryRepository,
                                    ICommodityTypeRepository commodityTypeRepository,
                                    ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder,
                                    ICustomerRepository customerRepository,
                                    ICustomerViewModelSelectListBuilder customerViewModelSelectListBuilder)
        {
            this.commodityCategoryRepository = commodityCategoryRepository;
            this.commodityCategorySelectListBuilder = commodityCategorySelectListBuilder;
            this.commodityTypeRepository = commodityTypeRepository;
            this.commodityTypeSelectListBuilder = commodityTypeSelectListBuilder;
            this.customerRepository = customerRepository;
            this.customerViewModelSelectListBuilder = customerViewModelSelectListBuilder;
        }

        public void BuildSelectLists(CommodityViewModel commodityViewModel)
        {
            commodityViewModel.CommodityCategoryDropDown = commodityCategorySelectListBuilder.BuildSelectListItemsForCommodityCategories(commodityCategoryRepository.GetAllCommodityCategories());
            commodityViewModel.CommodityTypeDropDown = commodityTypeSelectListBuilder.BuildSelectListItemsForCommodityCategories(commodityTypeRepository.GetAllCommodityTypes());
            commodityViewModel.SupplierDropDown = customerViewModelSelectListBuilder.BuildSelectListItemsSuppliers(customerRepository.GetAllSuppliers());
        }
        
    }
}