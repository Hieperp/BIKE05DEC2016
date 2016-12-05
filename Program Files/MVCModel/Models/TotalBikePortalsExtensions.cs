using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using MVCModel.Helpers;

namespace MVCModel.Models
{
    class TotalBikePortalsExtensions
    {
    }

    public partial class PurchaseOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<PurchaseOrderDetail>
    {
        public int GetID() { return this.PurchaseOrderID; }

        public ICollection<PurchaseOrderDetail> GetDetails() { return this.PurchaseOrderDetails; }
    }

    public partial class PurchaseOrderDetail : IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseOrderDetailID; }
    }


    public partial class PurchaseInvoice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<PurchaseInvoiceDetail>
    {
        public int GetID() { return this.PurchaseInvoiceID; }

        public ICollection<PurchaseInvoiceDetail> GetDetails() { return this.PurchaseInvoiceDetails; }
    }


    public partial class PurchaseInvoiceDetail : IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseInvoiceDetailID; }
    }


    public partial class GoodsReceipt : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsReceiptDetail>
    {
        public int GetID() { return this.GoodsReceiptID; }

        public ICollection<GoodsReceiptDetail> GetDetails() { return this.GoodsReceiptDetails; }
    }


    public partial class GoodsReceiptDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.GoodsReceiptDetailID; }
        public int GetWarehouseID() { return this.WarehouseID; }
    }



    public partial class SalesInvoice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SalesInvoiceDetail>
    {
        public int GetID() { return this.SalesInvoiceID; }

        public ICollection<SalesInvoiceDetail> GetDetails() { return this.SalesInvoiceDetails; }
    }


    public partial class SalesInvoiceDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.SalesInvoiceDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class AccountInvoice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<AccountInvoiceDetail>
    {
        public int GetID() { return this.AccountInvoiceID; }

        public ICollection<AccountInvoiceDetail> GetDetails() { return this.AccountInvoiceDetails; }
    }


    public partial class AccountInvoiceDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.AccountInvoiceDetailID; }
    }


    public partial class Quotation : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<QuotationDetail>
    {
        public int GetID() { return this.QuotationID; }

        public ICollection<QuotationDetail> GetDetails() { return this.QuotationDetails; }
    }


    public partial class QuotationDetail : IPrimitiveEntity
    {
        public int GetID() { return this.QuotationDetailID; }
    }



    public partial class TransferOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TransferOrderDetail>
    {
        public int GetID() { return this.TransferOrderID; }

        public ICollection<TransferOrderDetail> GetDetails() { return this.TransferOrderDetails; }
    }


    public partial class TransferOrderDetail : IPrimitiveEntity
    {
        public int GetID() { return this.TransferOrderDetailID; }
    }


    public partial class StockTransfer : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<StockTransferDetail>
    {
        public int GetID() { return this.StockTransferID; }

        public ICollection<StockTransferDetail> GetDetails() { return this.StockTransferDetails; }
    }


    public partial class StockTransferDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.StockTransferDetailID; }
        public int GetWarehouseID() { return this.WarehouseID; }
    }

    public partial class InventoryAdjustment : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<InventoryAdjustmentDetail>
    {
        public int GetID() { return this.InventoryAdjustmentID; }

        public ICollection<InventoryAdjustmentDetail> GetDetails() { return this.InventoryAdjustmentDetails; }
    }


    public partial class InventoryAdjustmentDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.InventoryAdjustmentDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class Customer : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CustomerID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }



    public partial class Commodity : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CommodityID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }





    public partial class EntireTerritory : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.TerritoryID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }




    public partial class ServiceContract : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.ServiceContractID; }
    }



}
