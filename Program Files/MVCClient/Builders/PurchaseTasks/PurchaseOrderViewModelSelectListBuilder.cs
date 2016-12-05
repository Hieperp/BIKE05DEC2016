using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.PurchaseTasks;

namespace MVCClient.Builders.PurchaseTasks
{
    public class PurchaseOrderViewModelSelectListBuilder : IPurchaseOrderViewModelSelectListBuilder
    {
        private readonly IPriceTermSelectListBuilder priceTermSelectListBuilder;
        private readonly IPriceTermRepository priceTermRepository;
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public PurchaseOrderViewModelSelectListBuilder(IPriceTermSelectListBuilder priceTermSelectListBuilder,
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

        public void BuildSelectLists(PurchaseOrderViewModel purchaseOrderViewModel)
        {
            purchaseOrderViewModel.PriceTermDropDown = priceTermSelectListBuilder.BuildSelectListItemsForPriceTerms(priceTermRepository.GetAllPriceTerms());
            purchaseOrderViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            purchaseOrderViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), purchaseOrderViewModel.UserID);
            purchaseOrderViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), purchaseOrderViewModel.UserID);
        }

    }
}