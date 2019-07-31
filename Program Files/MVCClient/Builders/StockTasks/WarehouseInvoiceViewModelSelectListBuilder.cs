using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.StockTasks;

namespace MVCClient.Builders.StockTasks
{
    public interface IWarehouseInvoiceViewModelSelectListBuilder : IViewModelSelectListBuilder<WarehouseInvoiceViewModel>
    {
    }

    public class WarehouseInvoiceViewModelSelectListBuilder : IWarehouseInvoiceViewModelSelectListBuilder
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public WarehouseInvoiceViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(WarehouseInvoiceViewModel warehouseInvoiceViewModel)
        {
            warehouseInvoiceViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), warehouseInvoiceViewModel.UserID);
            warehouseInvoiceViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), warehouseInvoiceViewModel.UserID);
        }

    }
}