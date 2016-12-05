using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.SalesTasks;

namespace MVCClient.Builders.SalesTasks
{
    public class AccountInvoiceViewModelSelectListBuilder : IAccountInvoiceViewModelSelectListBuilder
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public AccountInvoiceViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(AccountInvoiceViewModel accountInvoiceViewModel)
        {
            accountInvoiceViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), accountInvoiceViewModel.UserID);
            accountInvoiceViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), accountInvoiceViewModel.UserID);
        }

    }
}