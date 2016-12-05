using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.SalesTasks;

namespace MVCClient.Builders.SalesTasks
{
    public class QuotationViewModelSelectListBuilder : IQuotationViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public QuotationViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder,
                                    IPaymentTermRepository paymentTermRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(QuotationViewModel quotationViewModel)
        {
            quotationViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            quotationViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), quotationViewModel.UserID);
            quotationViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), quotationViewModel.UserID);
        }

    }
}