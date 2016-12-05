using MVCCore.Repositories.CommonTasks;

using MVCClient.Builders.CommonTasks;
using MVCClient.ViewModels.StockTasks;

namespace MVCClient.Builders.StockTasks
{
    public class VehicleAdjustmentViewModelSelectListBuilder : IVehicleAdjustmentViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public VehicleAdjustmentViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder,
                                    IPaymentTermRepository paymentTermRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(VehicleAdjustmentViewModel vehicleAdjustmentViewModel)
        {
            vehicleAdjustmentViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            vehicleAdjustmentViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), vehicleAdjustmentViewModel.UserID);
            vehicleAdjustmentViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), vehicleAdjustmentViewModel.UserID);
        }

    }

    public class PartAdjustmentViewModelSelectListBuilder : IPartAdjustmentViewModelSelectListBuilder
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public PartAdjustmentViewModelSelectListBuilder(IPaymentTermSelectListBuilder paymentTermSelectListBuilder,
                                    IPaymentTermRepository paymentTermRepository,
                                    IAspNetUserSelectListBuilder aspNetUserSelectListBuilder,
                                    IAspNetUserRepository aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public void BuildSelectLists(PartAdjustmentViewModel partAdjustmentViewModel)
        {
            partAdjustmentViewModel.PaymentTermDropDown = paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(paymentTermRepository.GetAllPaymentTerms());
            partAdjustmentViewModel.ApproverDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), partAdjustmentViewModel.UserID);
            partAdjustmentViewModel.PreparedPersonDropDown = aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(aspNetUserRepository.GetAllAspNetUsers(), partAdjustmentViewModel.UserID);
        }

    }

}