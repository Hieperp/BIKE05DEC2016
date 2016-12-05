using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.SalesTasks;

namespace MVCClient.Builders.SalesTasks
{
    public class VehiclesInvoiceViewModelSelectListBuilder : IVehiclesInvoiceViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public VehiclesInvoiceViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder,
                                    IPaymentTermRepository paymentTermRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(VehiclesInvoiceViewModel vehiclesInvoiceViewModel)
        {
            vehiclesInvoiceViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            vehiclesInvoiceViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), vehiclesInvoiceViewModel.UserID);
            vehiclesInvoiceViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), vehiclesInvoiceViewModel.UserID);
        }

    }

    public class PartsInvoiceViewModelSelectListBuilder : IPartsInvoiceViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public PartsInvoiceViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder,
                                    IPaymentTermRepository paymentTermRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(PartsInvoiceViewModel partsInvoiceViewModel)
        {
            partsInvoiceViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            partsInvoiceViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), partsInvoiceViewModel.UserID);
            partsInvoiceViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), partsInvoiceViewModel.UserID);
        }

    }


    public class ServicesInvoiceViewModelSelectListBuilder : IServicesInvoiceViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public ServicesInvoiceViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder,
                                    IPaymentTermRepository paymentTermRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(ServicesInvoiceViewModel servicesInvoiceViewModel)
        {
            servicesInvoiceViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            servicesInvoiceViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), servicesInvoiceViewModel.UserID);
            servicesInvoiceViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), servicesInvoiceViewModel.UserID);
        }

    }
}