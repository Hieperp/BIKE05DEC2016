using System;
using System.Linq;
using AutoMapper;
using MVCModel.Models;
using MVCClient.ViewModels.PurchaseTasks;
using MVCClient.ViewModels.SalesTasks;
using MVCClient.ViewModels.CommonTasks;
using MVCClient.ViewModels.Menus;
using MVCDTO.PurchaseTasks;
using MVCDTO.SalesTasks;
using MVCDTO.CommonTasks;
using System.Collections.Generic;
using MVCClient.ViewModels.StockTasks;
using MVCDTO.StockTasks;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MVCClient.App_Start.AutoMapperConfig), "SetupMappings")]
namespace MVCClient.App_Start
{
    public static class AutoMapperConfig
    {
        public static void SetupMappings()
        {
            Mapper.CreateMap<PurchaseOrder, PurchaseOrderViewModel>();
            Mapper.CreateMap<PurchaseOrderViewModel, PurchaseOrder>();
            Mapper.CreateMap<PurchaseOrder, PurchaseOrderDTO>();
            Mapper.CreateMap<PurchaseOrderPrimitiveDTO, PurchaseOrder>();


            Mapper.CreateMap<PurchaseOrder, PurchaseOrderPrimitiveDTO>();


            Mapper.CreateMap<PurchaseOrderDetail, PurchaseOrderDetailDTO>();
            Mapper.CreateMap<PurchaseOrderDetailDTO, PurchaseOrderDetail>();

            Mapper.CreateMap<PurchaseInvoice, PurchaseInvoiceViewModel>();
            Mapper.CreateMap<PurchaseInvoiceViewModel, PurchaseInvoice>();
            Mapper.CreateMap<PurchaseInvoice, PurchaseInvoiceDTO>();
            Mapper.CreateMap<PurchaseInvoicePrimitiveDTO, PurchaseInvoice>();
            Mapper.CreateMap<PurchaseInvoiceViewDetail, PurchaseInvoiceDetailDTO>();
            Mapper.CreateMap<PurchaseInvoiceDetailDTO, PurchaseInvoiceDetail>();

            Mapper.CreateMap<GoodsReceipt, GoodsReceiptViewModel>();
            Mapper.CreateMap<GoodsReceiptViewModel, GoodsReceipt>();
            Mapper.CreateMap<GoodsReceipt, GoodsReceiptDTO>();
            Mapper.CreateMap<GoodsReceiptPrimitiveDTO, GoodsReceipt>();
            Mapper.CreateMap<GoodsReceiptViewDetail, GoodsReceiptDetailDTO>();
            Mapper.CreateMap<GoodsReceiptDetailDTO, GoodsReceiptDetail>();

            Mapper.CreateMap<SalesInvoice, VehiclesInvoiceViewModel>();
            Mapper.CreateMap<SalesInvoice, VehiclesInvoiceDTO>();
            Mapper.CreateMap<VehiclesInvoiceViewModel, SalesInvoice>();
            Mapper.CreateMap<VehiclesInvoicePrimitiveDTO, SalesInvoice>();
            Mapper.CreateMap<VehiclesInvoiceViewDetail, VehiclesInvoiceDetailDTO>();
            Mapper.CreateMap<VehiclesInvoiceDetailDTO, SalesInvoiceDetail>();

            Mapper.CreateMap<SalesInvoice, PartsInvoiceViewModel>()
                .ForMember(des => des.ServiceInvoiceReference, m => m.MapFrom(src => src.SalesInvoice1.Reference))
                .ForMember(des => des.ServiceInvoiceEntryDate, m => m.MapFrom(src => src.SalesInvoice1.EntryDate));
            Mapper.CreateMap<SalesInvoice, PartsInvoiceDTO>();

            Mapper.CreateMap<PartsInvoiceViewModel, SalesInvoice>();
            Mapper.CreateMap<PartsInvoicePrimitiveDTO, SalesInvoice>();
            Mapper.CreateMap<PartsInvoiceViewDetail, PartsInvoiceDetailDTO>();
            Mapper.CreateMap<PartsInvoiceDetailDTO, SalesInvoiceDetail>();

            Mapper.CreateMap<SalesInvoice, ServicesInvoiceViewModel>()
                .ForMember(des => des.ReceptionistName, m => m.MapFrom(src => src.Employee1.Name));
            Mapper.CreateMap<SalesInvoice, ServicesInvoiceDTO>();
            Mapper.CreateMap<ServicesInvoiceViewModel, SalesInvoice>();
            Mapper.CreateMap<ServicesInvoicePrimitiveDTO, SalesInvoice>();
            Mapper.CreateMap<SalesInvoiceDetail, ServicesInvoiceDetailDTO>();
            Mapper.CreateMap<ServicesInvoiceDetailDTO, SalesInvoiceDetail>();


            Mapper.CreateMap<Quotation, QuotationViewModel>();
            Mapper.CreateMap<QuotationViewModel, Quotation>();
            Mapper.CreateMap<QuotationPrimitiveDTO, Quotation>();
            Mapper.CreateMap<QuotationViewDetail, QuotationDetailDTO>();
            Mapper.CreateMap<QuotationDetailDTO, QuotationDetail>();
            Mapper.CreateMap<QuotationViewDetail, QuotationDetailPopupDTO>();
            
            Mapper.CreateMap<AccountInvoice, AccountInvoiceViewModel>();
            Mapper.CreateMap<AccountInvoiceViewModel, AccountInvoice>();
            Mapper.CreateMap<AccountInvoice, AccountInvoiceDTO>();
            Mapper.CreateMap<AccountInvoicePrimitiveDTO, AccountInvoice>();
            Mapper.CreateMap<AccountInvoiceViewDetail, AccountInvoiceDetailDTO>();
            Mapper.CreateMap<AccountInvoiceDetailDTO, AccountInvoiceDetail>();

            Mapper.CreateMap<Customer, CustomerViewModel>();
            Mapper.CreateMap<CustomerViewModel, Customer>();
            Mapper.CreateMap<CustomerPrimitiveDTO, Customer>();

            Mapper.CreateMap<Commodity, CommodityViewModel>();
            Mapper.CreateMap<CommodityViewModel, Commodity>();
            Mapper.CreateMap<CommodityPrimitiveDTO, Commodity>();

            Mapper.CreateMap<ServiceContract, ServiceContractViewModel>();
            Mapper.CreateMap<ServiceContract, ServiceContractDTO>();
            Mapper.CreateMap<ServiceContractViewModel, ServiceContract>();
            Mapper.CreateMap<ServiceContractPrimitiveDTO, ServiceContract>();


            Mapper.CreateMap<TransferOrder, VehicleTransferOrderViewModel>();
            Mapper.CreateMap<VehicleTransferOrderViewModel, TransferOrder>();
            Mapper.CreateMap<TransferOrder, VehicleTransferOrderDTO>();
            Mapper.CreateMap<VehicleTransferOrderPrimitiveDTO, TransferOrder>();
            Mapper.CreateMap<VehicleTransferOrderViewDetail, VehicleTransferOrderDetailDTO>();
            Mapper.CreateMap<VehicleTransferOrderDetailDTO, TransferOrderDetail>();


            Mapper.CreateMap<TransferOrder, PartTransferOrderViewModel>();
            Mapper.CreateMap<PartTransferOrderViewModel, TransferOrder>();
            Mapper.CreateMap<TransferOrder, PartTransferOrderDTO>();
            Mapper.CreateMap<PartTransferOrderPrimitiveDTO, TransferOrder>();
            Mapper.CreateMap<PartTransferOrderViewDetail, PartTransferOrderDetailDTO>();
            Mapper.CreateMap<PartTransferOrderDetailDTO, TransferOrderDetail>();

            Mapper.CreateMap<StockTransfer, VehicleTransferViewModel>();
            Mapper.CreateMap<VehicleTransferViewModel, StockTransfer>();
            Mapper.CreateMap<StockTransfer, VehicleTransferDTO>();
            Mapper.CreateMap<VehicleTransferPrimitiveDTO, StockTransfer>();
            Mapper.CreateMap<VehicleTransferViewDetail, VehicleTransferDetailDTO>();
            Mapper.CreateMap<VehicleTransferDetailDTO, StockTransferDetail>();

            Mapper.CreateMap<StockTransfer, PartTransferViewModel>();            
            Mapper.CreateMap<PartTransferViewModel, StockTransfer>();
            Mapper.CreateMap<StockTransfer, PartTransferDTO>();
            Mapper.CreateMap<PartTransferPrimitiveDTO, StockTransfer>();
            Mapper.CreateMap<PartTransferViewDetail, PartTransferDetailDTO>();
            Mapper.CreateMap<PartTransferDetailDTO, StockTransferDetail>();







            Mapper.CreateMap<InventoryAdjustment, VehicleAdjustmentViewModel>();
            Mapper.CreateMap<InventoryAdjustment, VehicleAdjustmentDTO>();
            Mapper.CreateMap<VehicleAdjustmentViewModel, InventoryAdjustment>();
            Mapper.CreateMap<VehicleAdjustmentPrimitiveDTO, InventoryAdjustment>();
            Mapper.CreateMap<VehicleAdjustmentViewDetail, VehicleAdjustmentDetailDTO>();
            Mapper.CreateMap<VehicleAdjustmentDetailDTO, InventoryAdjustmentDetail>();

            Mapper.CreateMap<InventoryAdjustment, PartAdjustmentViewModel>();
            Mapper.CreateMap<InventoryAdjustment, PartAdjustmentDTO>();
            Mapper.CreateMap<PartAdjustmentViewModel, InventoryAdjustment>();
            Mapper.CreateMap<PartAdjustmentPrimitiveDTO, InventoryAdjustment>();
            Mapper.CreateMap<PartAdjustmentViewDetail, PartAdjustmentDetailDTO>();
            Mapper.CreateMap<PartAdjustmentDetailDTO, InventoryAdjustmentDetail>();





            Mapper.CreateMap<Module, ModuleViewModel>();
            Mapper.CreateMap<ModuleDetail, ModuleDetailViewModel>();
        }
    }
}