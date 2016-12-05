using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;
using MVCData.Helpers.SqlProgrammability;

namespace MVCData.Helpers.SqlProgrammability.PurchaseTasks
{
    public class PurchaseInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PurchaseInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPurchaseInvoiceIndexes();

            this.PurchaseInvoiceGetPurchaseOrders();
            this.PurchaseInvoiceGetSuppliers();

            this.GetPurchaseInvoiceViewDetails();
            this.PurchaseInvoiceSaveRelative();
            this.PurchaseInvoicePostSaveValidate();

            this.PurchaseInvoiceEditable();

            this.PurchaseInvoiceInitReference();
        }

        private void GetPurchaseInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PurchaseInvoices.PurchaseInvoiceID, CAST(PurchaseInvoices.EntryDate AS DATE) AS EntryDate, PurchaseInvoices.Reference, Locations.Code AS LocationCode, Suppliers.Name + ',    ' + Suppliers.AddressNo AS SupplierDescription, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, PurchaseOrders.ConfirmReference, PurchaseOrders.ConfirmDate, PurchaseInvoices.VATInvoiceNo, PurchaseInvoices.VATInvoiceDate, PurchaseInvoices.TotalQuantity, PurchaseInvoices.TotalGrossAmount, PurchaseInvoices.Description " + "\r\n";
            queryString = queryString + "       FROM        PurchaseInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON PurchaseInvoices.EntryDate >= @FromDate AND PurchaseInvoices.EntryDate <= @ToDate AND PurchaseInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PurchaseInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = PurchaseInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID INNER JOIN" + "\r\n";
            queryString = queryString + "                   PurchaseOrders ON PurchaseInvoices.PurchaseOrderID = PurchaseOrders.PurchaseOrderID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPurchaseInvoiceIndexes", queryString);
        }

        private void PurchaseInvoiceGetPurchaseOrders()
        {
            string queryString = " @LocationID int, @PurchaseInvoiceID int, @PurchaseOrderReference nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          PurchaseOrders.PurchaseOrderID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, PurchaseOrders.ConfirmReference AS PurchaseOrderConfirmReference, PurchaseOrders.ConfirmDate AS PurchaseOrderConfirmDate, PurchaseOrders.AttentionName, PurchaseOrders.Description, PurchaseOrders.Remarks, " + "\r\n";
            queryString = queryString + "                       PurchaseOrders.SupplierID, Customers.Name AS CustomerName, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM            PurchaseOrders INNER JOIN Customers ON (@PurchaseOrderReference = '' OR PurchaseOrders.Reference LIKE '%' + @PurchaseOrderReference + '%') AND PurchaseOrders.LocationID = @LocationID AND PurchaseOrders.SupplierID = Customers.CustomerID INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           PurchaseOrders.PurchaseOrderID IN  " + "\r\n";

            queryString = queryString + "                      (SELECT PurchaseOrderID FROM PurchaseOrderDetails WHERE ROUND(Quantity - QuantityInvoice, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT PurchaseOrderID FROM PurchaseInvoiceDetails WHERE PurchaseInvoiceID = @PurchaseInvoiceID)  " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("PurchaseInvoiceGetPurchaseOrders", queryString);
        }

        private void PurchaseInvoiceGetSuppliers()
        {
            string queryString = " @LocationID int, @PurchaseInvoiceID int, @SupplierName nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID AS SupplierID, Customers.Name AS CustomerName, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM            Customers INNER JOIN EntireTerritories ON (@SupplierName = '' OR Customers.Name LIKE '%' + @SupplierName + '%') AND Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           CustomerID IN   " + "\r\n";

            queryString = queryString + "                      (SELECT SupplierID FROM PurchaseOrderDetails WHERE LocationID = @LocationID AND ROUND(Quantity - QuantityInvoice, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT SupplierID FROM PurchaseInvoices WHERE PurchaseInvoiceID = @PurchaseInvoiceID) " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("PurchaseInvoiceGetSuppliers", queryString);
        }



        private void GetPurchaseInvoiceViewDetails()
        {
            string queryString; string queryEdit; string queryNew;

            queryNew = "                SELECT          PurchaseOrders.PurchaseOrderID, PurchaseOrders.EntryDate AS PurchaseOrderDate, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.ConfirmReference, 0 AS PurchaseInvoiceDetailID, 0 AS PurchaseInvoiceID, PurchaseOrderDetails.PurchaseOrderDetailID, " + "\r\n";
            queryNew = queryNew + "                     PurchaseOrderDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, PurchaseOrderDetails.Origin, PurchaseOrderDetails.Packing, ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityInvoice, 0) AS QuantityRemains, " + "\r\n";
            queryNew = queryNew + "                     0.0 AS Quantity, PurchaseOrderDetails.UnitPrice, PurchaseOrderDetails.VATPercent, PurchaseOrderDetails.GrossPrice, 0.0 AS Amount, 0.0 AS VATAmount, 0.0 AS GrossAmount, " + "\r\n";
            queryNew = queryNew + "                     PurchaseOrderDetails.ChassisCode, PurchaseOrderDetails.EngineCode, PurchaseOrderDetails.ColorCode, PurchaseOrderDetails.Remarks " + "\r\n";

            queryNew = queryNew + "     FROM            PurchaseOrders INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     PurchaseOrderDetails ON PurchaseOrders.PurchaseOrderID = PurchaseOrderDetails.PurchaseOrderID INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     Commodities ON PurchaseOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryNew = queryNew + "     WHERE           (@PurchaseOrderID = 0 OR PurchaseOrderDetails.PurchaseOrderID = @PurchaseOrderID) AND (@SupplierID = 0 OR PurchaseOrders.SupplierID = @SupplierID) AND ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityInvoice, 0) > 0 " + "\r\n"; //AND PurchaseOrderDetailMaster.Approved = 1 


            queryEdit = "               SELECT          PurchaseOrders.PurchaseOrderID, PurchaseOrders.EntryDate AS PurchaseOrderDate, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.ConfirmReference, PurchaseInvoiceDetails.PurchaseInvoiceDetailID, PurchaseInvoiceDetails.PurchaseInvoiceID, PurchaseOrderDetails.PurchaseOrderDetailID, " + "\r\n";
            queryEdit = queryEdit + "                   PurchaseOrderDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, PurchaseInvoiceDetails.Origin, PurchaseInvoiceDetails.Packing, ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityInvoice + PurchaseInvoiceDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryEdit = queryEdit + "                   PurchaseInvoiceDetails.Quantity, PurchaseInvoiceDetails.UnitPrice, PurchaseInvoiceDetails.VATPercent, PurchaseInvoiceDetails.GrossPrice, PurchaseInvoiceDetails.Amount, PurchaseInvoiceDetails.VATAmount, PurchaseInvoiceDetails.GrossAmount, " + "\r\n";
            queryEdit = queryEdit + "                   PurchaseInvoiceDetails.ChassisCode, PurchaseInvoiceDetails.EngineCode, PurchaseInvoiceDetails.ColorCode, PurchaseInvoiceDetails.Remarks " + "\r\n";

            queryEdit = queryEdit + "   FROM            PurchaseOrders INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   PurchaseOrderDetails ON PurchaseOrders.PurchaseOrderID = PurchaseOrderDetails.PurchaseOrderID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   PurchaseInvoiceDetails ON PurchaseOrderDetails.PurchaseOrderDetailID = PurchaseInvoiceDetails.PurchaseOrderDetailID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Commodities ON PurchaseInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryEdit = queryEdit + "   WHERE           (@PurchaseOrderID = 0 OR PurchaseOrderDetails.PurchaseOrderID = @PurchaseOrderID) AND (@SupplierID = 0 OR PurchaseOrders.SupplierID = @SupplierID) AND PurchaseInvoiceDetails.PurchaseInvoiceID = @PurchaseInvoiceID " + "\r\n";



            queryString = " @PurchaseInvoiceID Int, @PurchaseOrderID Int, @SupplierID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + " IF (@PurchaseInvoiceID <= 0) " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       " + queryNew + "\r\n";
            queryString = queryString + "        ORDER BY PurchaseOrders.EntryDate, PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";
            queryString = queryString + " ELSE " + "\r\n";

            queryString = queryString + "   IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           " + queryEdit + "\r\n";
            queryString = queryString + "           ORDER BY PurchaseOrders.EntryDate, PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            queryString = queryString + "   ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           " + queryNew + " AND PurchaseOrderDetails.PurchaseOrderDetailID NOT IN (SELECT PurchaseOrderDetailID FROM PurchaseInvoiceDetails WHERE PurchaseInvoiceID = @PurchaseInvoiceID) " + "\r\n";
            queryString = queryString + "           UNION ALL " + "\r\n";
            queryString = queryString + "           " + queryEdit + "\r\n";
            queryString = queryString + "           ORDER BY PurchaseOrders.EntryDate, PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPurchaseInvoiceViewDetails", queryString);

        }


        private void PurchaseInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE          PurchaseOrderDetails " + "\r\n";
            queryString = queryString + "       SET             PurchaseOrderDetails.QuantityInvoice = ROUND(PurchaseOrderDetails.QuantityInvoice + PurchaseInvoiceDetails.Quantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            PurchaseInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       PurchaseOrderDetails ON PurchaseInvoiceDetails.PurchaseInvoiceID = @EntityID AND PurchaseInvoiceDetails.PurchaseOrderDetailID = PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("PurchaseInvoiceSaveRelative", queryString);

        }

        private void PurchaseInvoicePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Order Date: ' + CAST(PurchaseOrders.EntryDate AS nvarchar) FROM PurchaseInvoiceDetails INNER JOIN PurchaseOrders ON PurchaseInvoiceDetails.PurchaseInvoiceID = @EntityID AND PurchaseInvoiceDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID AND PurchaseInvoiceDetails.EntryDate < PurchaseOrders.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityInvoice, 0) AS nvarchar) FROM PurchaseOrderDetails WHERE (ROUND(Quantity - QuantityInvoice, 0) < 0) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PurchaseInvoicePostSaveValidate", queryArray);
        }


        private void PurchaseInvoiceEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = VoucherID FROM GoodsReceipts WHERE GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND VoucherID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PurchaseInvoiceEditable", queryArray);
        }

        private void PurchaseInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("PurchaseInvoices", "PurchaseInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.PurchaseInvoice));
            this.totalBikePortalsEntities.CreateTrigger("PurchaseInvoiceInitReference", simpleInitReference.CreateQuery());
        }


    }
}
