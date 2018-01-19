using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.SalesTasks;

namespace MVCClient.Builders.SalesTasks
{
    public class AccountInvoiceViewModelSelectListBuilder : IAccountInvoiceViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public AccountInvoiceViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository, IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(AccountInvoiceViewModel accountInvoiceViewModel)
        {
            accountInvoiceViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            accountInvoiceViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), accountInvoiceViewModel.UserID);
            accountInvoiceViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), accountInvoiceViewModel.UserID);
        }

    }
}