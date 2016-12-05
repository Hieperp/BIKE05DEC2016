[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MVCClient.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MVCClient.App_Start.NinjectWebCommon), "Stop")]

namespace MVCClient.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using MVCModel.Models;
    using MVCCore.Repositories.CommonTasks;
    using MVCCore.Repositories.PurchaseTasks;
    using MVCCore.Repositories.SalesTasks;
    using MVCCore.Services.CommonTasks;
    using MVCCore.Services.PurchaseTasks;
    using MVCCore.Services.SalesTasks;
    using MVCData.Repositories.CommonTasks;
    using MVCData.Repositories.PurchaseTasks;
    using MVCData.Repositories.SalesTasks;
    using MVCClient.Builders;
    using MVCClient.Builders.PurchaseTasks;
    using MVCClient.Builders.SalesTasks;
    using MVCClient.Builders.CommonTasks;
    using MVCClient.Converters;
    using MVCClient.ViewModels.PurchaseTasks;
    using MVCClient.ViewModels.SalesTasks;
    using MVCCore.Helpers;
    using MVCData.Helpers;
    using MVCClient.ViewModels.Menus;
    using MVCService.CommonTasks; 
    using MVCService.PurchaseTasks;
    using MVCService.SalesTasks;
    using MVCDTO.PurchaseTasks;
    using MVCDTO.SalesTasks;
    using MVCCore.Services.StockTasks;
    using MVCService.StockTasks;
    using MVCCore.Repositories.StockTasks;
    using MVCData.Repositories.StockTasks;
    using MVCClient.Builders.StockTasks;
    using MVCCore.Services.Helpers;
    using MVCService.Helpers;
    using MVCCore.Repositories.Analysis;
    using MVCData.Repositories.Analysis;
    using MVCCore.Repositories;
    using MVCData.Repositories;
       

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                kernel.Bind<TotalBikePortalsEntities>().ToSelf().InRequestScope();

                kernel.Bind<IBaseRepository>().To<BaseRepository>();
                kernel.Bind<IReportRepository>().To<ReportRepository>();

                kernel.Bind<IPurchaseOrderService>().To<PurchaseOrderService>();
                kernel.Bind<IPurchaseOrderRepository>().To<PurchaseOrderRepository>();
                kernel.Bind<IPurchaseOrderAPIRepository>().To<PurchaseOrderAPIRepository>();

                kernel.Bind<IPurchaseInvoiceService>().To<PurchaseInvoiceService>();                
                kernel.Bind<IPurchaseInvoiceRepository>().To<PurchaseInvoiceRepository>();
                kernel.Bind<IPurchaseInvoiceAPIRepository>().To<PurchaseInvoiceAPIRepository>();

                kernel.Bind<IGoodsReceiptService>().To<GoodsReceiptService>();
                kernel.Bind<IGoodsReceiptRepository>().To<GoodsReceiptRepository>();
                kernel.Bind<IGoodsReceiptAPIRepository>().To<GoodsReceiptAPIRepository>();
                kernel.Bind<IGoodsReceiptHelperService>().To<GoodsReceiptHelperService>();

                kernel.Bind<IVehiclesInvoiceService>().To<VehiclesInvoiceService>();
                kernel.Bind<IVehiclesInvoiceRepository>().To<VehiclesInvoiceRepository>();
                kernel.Bind<IVehiclesInvoiceAPIRepository>().To<VehiclesInvoiceAPIRepository>();

                kernel.Bind<IPartsInvoiceService>().To<PartsInvoiceService>();
                kernel.Bind<IPartsInvoiceRepository>().To<PartsInvoiceRepository>();
                kernel.Bind<IPartsInvoiceAPIRepository>().To<PartsInvoiceAPIRepository>();
                kernel.Bind<IPartsInvoiceHelperService>().To<PartsInvoiceHelperService>();

                kernel.Bind<IServicesInvoiceService>().To<ServicesInvoiceService>();
                kernel.Bind<IServicesInvoiceRepository>().To<ServicesInvoiceRepository>();
                kernel.Bind<IServicesInvoiceAPIRepository>().To<ServicesInvoiceAPIRepository>();

                kernel.Bind<IQuotationService>().To<QuotationService>();
                kernel.Bind<IQuotationRepository>().To<QuotationRepository>();

                kernel.Bind<IAccountInvoiceService>().To<AccountInvoiceService>();
                kernel.Bind<IAccountInvoiceRepository>().To<AccountInvoiceRepository>();
                kernel.Bind<IAccountInvoiceAPIRepository>().To<AccountInvoiceAPIRepository>();

                kernel.Bind<ICustomerService>().To<CustomerService>();
                kernel.Bind<ICustomerRepository>().To<CustomerRepository>();

                kernel.Bind<ICommodityService>().To<CommodityService>();
                kernel.Bind<ICommodityRepository>().To<CommodityRepository>(); 

                kernel.Bind<IServiceContractService>().To<ServiceContractService>();
                kernel.Bind<IServiceContractRepository>().To<ServiceContractRepository>();
                kernel.Bind<IServiceContractAPIRepository>().To<ServiceContractAPIRepository>();

                kernel.Bind<IVehicleTransferOrderService>().To<VehicleTransferOrderService>();
                kernel.Bind<IVehicleTransferOrderRepository>().To<VehicleTransferOrderRepository>();
                kernel.Bind<IVehicleTransferOrderAPIRepository>().To<VehicleTransferOrderAPIRepository>();

                kernel.Bind<IPartTransferOrderService>().To<PartTransferOrderService>();
                kernel.Bind<IPartTransferOrderRepository>().To<PartTransferOrderRepository>();
                kernel.Bind<IPartTransferOrderAPIRepository>().To<PartTransferOrderAPIRepository>();

                kernel.Bind<IVehicleTransferService>().To<VehicleTransferService>();
                kernel.Bind<IVehicleTransferRepository>().To<VehicleTransferRepository>();
                kernel.Bind<IVehicleTransferAPIRepository>().To<VehicleTransferAPIRepository>();

                kernel.Bind<IPartTransferService>().To<PartTransferService>();
                kernel.Bind<IPartTransferRepository>().To<PartTransferRepository>();
                kernel.Bind<IPartTransferAPIRepository>().To<PartTransferAPIRepository>();

                kernel.Bind<IPartTransferHelperService>().To<PartTransferHelperService>();


                kernel.Bind<IPartAdjustmentService>().To<PartAdjustmentService>();
                kernel.Bind<IPartAdjustmentRepository>().To<PartAdjustmentRepository>();
                kernel.Bind<IPartAdjustmentAPIRepository>().To<PartAdjustmentAPIRepository>();
                kernel.Bind<IPartAdjustmentHelperService>().To<PartAdjustmentHelperService>();


                kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
                kernel.Bind<IPriceTermRepository>().To<PriceTermRepository>();
                kernel.Bind<IPaymentTermRepository>().To<PaymentTermRepository>();
                kernel.Bind<IPromotionRepository>().To<PromotionRepository>();

                kernel.Bind<IEntireTerritoryRepository>().To<EntireTerritoryRepository>();
                kernel.Bind<IInventoryRepository>().To<InventoryRepository>();
                kernel.Bind<ILocationRepository>().To<LocationRepository>();
                kernel.Bind<IWarehouseRepository>().To<WarehouseRepository>();
                kernel.Bind<ICommodityCategoryRepository>().To<CommodityCategoryRepository>();
                kernel.Bind<ICommodityTypeRepository>().To<CommodityTypeRepository>();
                kernel.Bind<ICustomerCategoryRepository>().To<CustomerCategoryRepository>();
                kernel.Bind<ICustomerTypeRepository>().To<CustomerTypeRepository>();
                kernel.Bind<ITerritoryRepository>().To<TerritoryRepository>();
                kernel.Bind<IAspNetUserRepository>().To<AspNetUserRepository>();
                kernel.Bind<IServiceContractTypeRepository>().To<ServiceContractTypeRepository>();
                kernel.Bind<IModuleRepository>().To<ModuleRepository>();
                kernel.Bind<IModuleDetailRepository>().To<ModuleDetailRepository>();

                kernel.Bind<IPurchaseOrderViewModelSelectListBuilder>().To<PurchaseOrderViewModelSelectListBuilder>();
                kernel.Bind<IPurchaseInvoiceViewModelSelectListBuilder>().To<PurchaseInvoiceViewModelSelectListBuilder>();
                kernel.Bind<IGoodsReceiptViewModelSelectListBuilder>().To<GoodsReceiptViewModelSelectListBuilder>();
                kernel.Bind<IVehiclesInvoiceViewModelSelectListBuilder>().To<VehiclesInvoiceViewModelSelectListBuilder>();
                kernel.Bind<IPartsInvoiceViewModelSelectListBuilder>().To<PartsInvoiceViewModelSelectListBuilder>();
                kernel.Bind<IServicesInvoiceViewModelSelectListBuilder>().To<ServicesInvoiceViewModelSelectListBuilder>();
                kernel.Bind<IQuotationViewModelSelectListBuilder>().To<QuotationViewModelSelectListBuilder>();
                kernel.Bind<IAccountInvoiceViewModelSelectListBuilder>().To<AccountInvoiceViewModelSelectListBuilder>();

                kernel.Bind<ICommodityViewModelSelectListBuilder>().To<CommodityViewModelSelectListBuilder>();
                kernel.Bind<ICommodityCategorySelectListBuilder>().To<CommodityCategorySelectListBuilder>();
                kernel.Bind<ICommodityTypeSelectListBuilder>().To<CommodityTypeSelectListBuilder>();

                kernel.Bind<IVehicleTransferOrderViewModelSelectListBuilder>().To<VehicleTransferOrderViewModelSelectListBuilder>();
                kernel.Bind<IPartTransferOrderViewModelSelectListBuilder>().To<PartTransferOrderViewModelSelectListBuilder>();

                kernel.Bind<IVehicleTransferViewModelSelectListBuilder>().To<VehicleTransferViewModelSelectListBuilder>();
                kernel.Bind<IPartTransferViewModelSelectListBuilder>().To<PartTransferViewModelSelectListBuilder>();

                kernel.Bind<IPartAdjustmentViewModelSelectListBuilder>().To<PartAdjustmentViewModelSelectListBuilder>();

                kernel.Bind<IPriceTermSelectListBuilder>().To<PriceTermSelectListBuilder>();
                kernel.Bind<IPaymentTermSelectListBuilder>().To<PaymentTermSelectListBuilder>();
                kernel.Bind<IAspNetUserSelectListBuilder>().To<AspNetUserSelectListBuilder>();

                kernel.Bind<ICustomerViewModelSelectListBuilder>().To<CustomerViewModelSelectListBuilder>();
                kernel.Bind<ICustomerCategorySelectListBuilder>().To<CustomerCategorySelectListBuilder>();
                kernel.Bind<ICustomerTypeSelectListBuilder>().To<CustomerTypeSelectListBuilder>();

                kernel.Bind<IServiceContractViewModelSelectListBuilder>().To<ServiceContractViewModelSelectListBuilder>();
                kernel.Bind<IServiceContractTypeSelectListBuilder>().To<ServiceContractTypeSelectListBuilder>();
                kernel.Bind<IWarehouseSelectListBuilder>().To<WarehouseSelectListBuilder>();
                
                kernel.Bind<IConverter<ModuleViewModel, Module>>().To<ModuleViewModelConverter>();
                kernel.Bind<IConverter<ModuleDetailViewModel, ModuleDetail>>().To<ModuleDetailViewModelConverter>();

                
                

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
