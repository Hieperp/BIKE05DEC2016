using MVCCore.Repositories.CommonTasks;
using MVCCore.Repositories.PurchaseTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.PurchaseTasks;

namespace MVCClient.Builders.PurchaseTasks
{
    public class PurchaseInvoiceViewModelSelectListBuilder : IPurchaseInvoiceViewModelSelectListBuilder
    {
        private readonly IPriceTermSelectListBuilder priceTermSelectListBuilder;
        private readonly IPriceTermRepository priceTermRepository;
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;               

        public PurchaseInvoiceViewModelSelectListBuilder(IPriceTermSelectListBuilder priceTermSelectListBuilder,
                                    IPriceTermRepository priceTermRepository,
                                    IPaymentTermSelectListBuilder paymentTermSelectListBuilder,
                                    IPaymentTermRepository paymentTermRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.priceTermSelectListBuilder = priceTermSelectListBuilder;
            this.priceTermRepository = priceTermRepository;
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(PurchaseInvoiceViewModel purchaseInvoiceViewModel)
        {
            purchaseInvoiceViewModel.PriceTermDropDown = priceTermSelectListBuilder.BuildSelectListItemsForPriceTerms(priceTermRepository.GetAllPriceTerms());
            purchaseInvoiceViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            purchaseInvoiceViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), purchaseInvoiceViewModel.UserID);
            purchaseInvoiceViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), purchaseInvoiceViewModel.UserID);            
        }
    }
}