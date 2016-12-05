using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class GoodsReceipt
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public GoodsReceipt(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsReceiptIndexes();

            this.GoodsReceiptGetPurchaseInvoices();
            this.GoodsReceiptGetStockTransfers();
            this.GetAdditionalGoodsReceiptVoucherText();

            this.GetGoodsReceiptViewDetails();
            this.GoodsReceiptSaveRelative();
            this.GoodsReceiptPostSaveValidate();

            this.GoodsReceiptEditable();

            this.GoodsReceiptInitReference();
        }

        private void GetGoodsReceiptIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Locations.Code AS LocationCode, GoodsReceipts.GoodsReceiptID, CAST(GoodsReceipts.EntryDate AS DATE) AS EntryDate, GoodsReceipts.Reference, GoodsReceipts.Description, GoodsReceipts.TotalQuantity, 'XE' AS VoucherType, PurchaseInvoices.VATInvoiceNo AS VoucherReference, PurchaseInvoices.VATInvoiceDate AS VoucherDate, Suppliers.OfficialName AS VoucherOwner " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceipts INNER JOIN " + "\r\n";
            queryString = queryString + "                   PurchaseInvoices ON GoodsReceipts.EntryDate >= @FromDate AND GoodsReceipts.EntryDate <= @ToDate AND GoodsReceipts.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND GoodsReceipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.GoodsReceipt + " AND AccessControls.AccessLevel > 0) AND GoodsReceipts.VoucherID = PurchaseInvoices.PurchaseInvoiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Locations ON GoodsReceipts.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "       UNION ALL" + "\r\n";

            queryString = queryString + "       SELECT      Locations.Code AS LocationCode, GoodsReceipts.GoodsReceiptID, CAST(GoodsReceipts.EntryDate AS DATE) AS EntryDate, GoodsReceipts.Reference, GoodsReceipts.Description, GoodsReceipts.TotalQuantity, 'PT' AS VoucherType, StockTransfers.Reference AS VoucherReference, StockTransfers.EntryDate AS VoucherDate, StockTransferLocations.Name AS VoucherOwner " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceipts INNER JOIN " + "\r\n";
            queryString = queryString + "                   StockTransfers ON GoodsReceipts.EntryDate >= @FromDate AND GoodsReceipts.EntryDate <= @ToDate AND GoodsReceipts.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND GoodsReceipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.GoodsReceipt + " AND AccessControls.AccessLevel > 0) AND GoodsReceipts.VoucherID = StockTransfers.StockTransferID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Locations StockTransferLocations ON StockTransfers.LocationID = StockTransferLocations.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Locations ON GoodsReceipts.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetGoodsReceiptIndexes", queryString);
        }

        private void GoodsReceiptGetPurchaseInvoices()
        {
            string queryString = " @LocationID int, @GoodsReceiptID int, @PurchaseInvoiceReference nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT         " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AS GoodsReceiptTypeID, PurchaseInvoices.PurchaseInvoiceID AS VoucherID, PurchaseInvoices.EntryDate, PurchaseInvoices.Reference, PurchaseInvoices.VATInvoiceNo, PurchaseInvoices.VATInvoiceDate, Customers.Name AS CustomerName, PurchaseInvoices.AttentionName, Customers.Telephone, PurchaseInvoices.Description, PurchaseInvoices.Remarks " + "\r\n";
            queryString = queryString + "       FROM            PurchaseInvoices INNER JOIN Customers ON (@PurchaseInvoiceReference = '' OR PurchaseInvoices.Reference LIKE '%' + @PurchaseInvoiceReference + '%' OR PurchaseInvoices.VATInvoiceNo LIKE '%' + @PurchaseInvoiceReference + '%') AND PurchaseInvoices.LocationID = @LocationID AND PurchaseInvoices.SupplierID = Customers.CustomerID INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           PurchaseInvoices.PurchaseInvoiceID IN  " + "\r\n";

            queryString = queryString + "                      (SELECT PurchaseInvoiceID FROM PurchaseInvoiceDetails WHERE ROUND(Quantity - QuantityReceipt, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT VoucherID FROM GoodsReceipts WHERE GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND GoodsReceiptID = @GoodsReceiptID)  " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GoodsReceiptGetPurchaseInvoices", queryString);
        }

        private void GoodsReceiptGetStockTransfers()
        {
            string queryString = " @LocationID int, @GoodsReceiptID int, @StockTransferReference nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AS GoodsReceiptTypeID, StockTransfers.StockTransferID AS VoucherID, StockTransfers.EntryDate, StockTransfers.Reference, StockTransferTypes.Code AS StockTransferTypeCode, Locations.Name AS LocationName, Locations.Telephone, Locations.Facsimile, Locations.Address, StockTransfers.Description, StockTransfers.Remarks " + "\r\n";
            queryString = queryString + "       FROM            StockTransfers INNER JOIN StockTransferTypes ON StockTransfers.StockTransferTypeID = StockTransferTypes.StockTransferTypeID INNER JOIN Locations ON StockTransfers.LocationID = Locations.LocationID AND StockTransfers.WarehouseID IN (SELECT WarehouseID FROM Warehouses WHERE LocationID = @LocationID) AND (@StockTransferReference = '' OR StockTransfers.Reference LIKE '%' + @StockTransferReference + '%') " + "\r\n";

            queryString = queryString + "       WHERE           StockTransfers.StockTransferID IN  " + "\r\n";

            queryString = queryString + "                      (SELECT StockTransferID FROM StockTransferDetails WHERE ROUND(Quantity - QuantityReceipt, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT VoucherID FROM GoodsReceipts WHERE GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND GoodsReceiptID = @GoodsReceiptID)  " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GoodsReceiptGetStockTransfers", queryString);
        }

        private void GetAdditionalGoodsReceiptVoucherText()
        {
            string queryString = " @GoodsReceiptTypeID int, @VoucherID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + ") \r\n";

            queryString = queryString + "           SELECT          PurchaseInvoices.Reference, PurchaseInvoices.EntryDate, PurchaseInvoices.VATInvoiceNo, PurchaseInvoices.VATInvoiceDate, Customers.Name AS VoucherText1, PurchaseInvoices.AttentionName AS VoucherText2, Customers.Telephone AS VoucherText3, Customers.AddressNo + ', ' + EntireTerritories.EntireName AS VoucherText4 " + "\r\n";
            queryString = queryString + "           FROM            PurchaseInvoices INNER JOIN Customers ON PurchaseInvoices.PurchaseInvoiceID = @VoucherID AND PurchaseInvoices.SupplierID = Customers.CustomerID INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       ELSE \r\n"; //(int)GlobalEnums.GoodsReceiptTypeID.StockTransfer

            queryString = queryString + "           SELECT          StockTransfers.Reference, StockTransfers.EntryDate, NULL AS VATInvoiceNo, NULL AS VATInvoiceDate, Locations.Name AS VoucherText1, Locations.Telephone AS VoucherText2, Locations.Facsimile AS VoucherText3, Locations.Address AS VoucherText4 " + "\r\n";
            queryString = queryString + "           FROM            StockTransfers INNER JOIN Locations ON StockTransfers.StockTransferID = @VoucherID AND StockTransfers.LocationID = Locations.LocationID " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetAdditionalGoodsReceiptVoucherText", queryString);
        }




        private void GetGoodsReceiptViewDetails()
        {
            string queryString;

            queryString = " @GoodsReceiptID Int, @GoodsReceiptTypeID int, @VoucherID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       " + GetGoodsReceiptViewDetailsBuilSQL(GlobalEnums.GoodsReceiptTypeID.StockTransfer);
            queryString = queryString + "       " + GetGoodsReceiptViewDetailsBuilSQL(GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice);
            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetGoodsReceiptViewDetails", queryString);
        }

        private string GetGoodsReceiptViewDetailsBuilSQL(GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID)
        {
            string queryString; string queryEdit; string queryNew = ""; string querySort;

            string entity = goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ? "PurchaseInvoices" : goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer ? "StockTransfers" : "";
            string entityDetail = goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ? "PurchaseInvoiceDetails" : goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer ? "StockTransferDetails" : "";
            string entityID = goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ? "PurchaseInvoiceID" : goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer ? "StockTransferID" : "";
            string entityDetailID = goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ? "PurchaseInvoiceDetailID" : goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer ? "StockTransferDetailID" : "";


            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice)
            {
                queryNew = "                SELECT          " + entityDetail + "." + entityID + " AS VoucherID, 0 AS GoodsReceiptDetailID, 0 AS GoodsReceiptID, " + entityDetail + "." + entityDetailID + " AS VoucherDetailID, " + "\r\n";
                queryNew = queryNew + "                     " + entityDetail + ".SupplierID, " + entityDetail + ".CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + entityDetail + ".Origin, " + entityDetail + ".Packing, ROUND(" + entityDetail + ".Quantity - " + entityDetail + ".QuantityReceipt, 0) AS QuantityRemains, " + "\r\n";
                queryNew = queryNew + "                     0.0 AS Quantity, " + entityDetail + ".UnitPrice, " + entityDetail + ".VATPercent, " + entityDetail + ".GrossPrice, 0.0 AS Amount, 0.0 AS VATAmount, 0.0 AS GrossAmount, " + "\r\n";
                queryNew = queryNew + "                     LWarehouses.WarehouseID, LWarehouses.Code AS WarehouseCode, " + entityDetail + ".ChassisCode, " + entityDetail + ".EngineCode, " + entityDetail + ".ColorCode, " + entityDetail + ".Remarks " + "\r\n";

                queryNew = queryNew + "     FROM            " + entityDetail + " INNER JOIN " + "\r\n";
                queryNew = queryNew + "                     Commodities ON " + entityDetail + ".CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
                queryNew = queryNew + "                     (SELECT DISTINCT LocationID, WarehouseID, Code FROM Warehouses) LWarehouses ON " + entityDetail + ".LocationID = LWarehouses.LocationID " + "\r\n"; //INIT Warehouses BY LOCATION: NOTE: IN ACCESS CONTROL: USER SHOULD ONLY HAVE EDITABLE PERMISSION IN THIER LOCATIONID ONLY

                queryNew = queryNew + "     WHERE           (@VoucherID = 0 OR " + entityDetail + "." + entityID + " = @VoucherID) AND ROUND(" + entityDetail + ".Quantity - " + entityDetail + ".QuantityReceipt, 0) > 0 " + "\r\n"; //These WHERE CLAUSE are the same WHEN ANY goodsReceiptTypeID //AND Approved = 1 
            }
            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer)
            {
                queryNew = "                SELECT          " + entityDetail + "." + entityID + " AS VoucherID, 0 AS GoodsReceiptDetailID, 0 AS GoodsReceiptID, " + entityDetail + "." + entityDetailID + " AS VoucherDetailID, " + "\r\n";
                queryNew = queryNew + "                     " + entityDetail + ".SupplierID, " + entityDetail + ".CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsReceiptDetails.Origin, GoodsReceiptDetails.Packing, ROUND(" + entityDetail + ".Quantity - " + entityDetail + ".QuantityReceipt, 0) AS QuantityRemains, " + "\r\n";
                queryNew = queryNew + "                     0.0 AS Quantity, ISNULL(GoodsReceiptDetails.UnitPrice, 0.0) AS UnitPrice, ISNULL(GoodsReceiptDetails.VATPercent, 0.0) AS VATPercent, ISNULL(GoodsReceiptDetails.GrossPrice, 0.0) AS GrossPrice, 0.0 AS Amount, 0.0 AS VATAmount, 0.0 AS GrossAmount, " + "\r\n";
                queryNew = queryNew + "                     " + entity + ".WarehouseID, Warehouses.Code AS WarehouseCode, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + entityDetail + ".Remarks " + "\r\n";

                queryNew = queryNew + "     FROM            " + entity + " INNER JOIN " + "\r\n";
                queryNew = queryNew + "                     " + entityDetail + " ON " + entity + "." + entityID + " = " + entityDetail + "." + entityID + " INNER JOIN " + "\r\n";
                queryNew = queryNew + "                     Warehouses ON " + entity + ".WarehouseID = Warehouses.WarehouseID INNER JOIN " + "\r\n";//Need to join Warehouses to get: WarehouseID, Warehouses.Name: NOT ALLOW CHANGE IN GoodsReceipts WHEN the goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer
                queryNew = queryNew + "                     Commodities ON " + entityDetail + ".CommodityID = Commodities.CommodityID LEFT JOIN " + "\r\n";
                queryNew = queryNew + "                     GoodsReceiptDetails ON " + entityDetail + ".GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n"; //Need to left join GoodsReceiptDetails to get: Origin, ChassisCode, ... (VehicleTransfer)

                queryNew = queryNew + "     WHERE           (@VoucherID = 0 OR " + entityDetail + "." + entityID + " = @VoucherID) AND ROUND(" + entityDetail + ".Quantity - " + entityDetail + ".QuantityReceipt, 0) > 0 " + "\r\n"; //These WHERE CLAUSE are the same WHEN ANY goodsReceiptTypeID //AND Approved = 1 
            }


            queryEdit = "               SELECT          " + entityDetail + "." + entityID + " AS VoucherID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.GoodsReceiptID, " + entityDetail + "." + entityDetailID + " AS VoucherDetailID, " + "\r\n";
            queryEdit = queryEdit + "                   GoodsReceiptDetails.SupplierID, " + entityDetail + ".CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsReceiptDetails.Origin, GoodsReceiptDetails.Packing, ROUND(" + entityDetail + ".Quantity - " + entityDetail + ".QuantityReceipt + GoodsReceiptDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryEdit = queryEdit + "                   GoodsReceiptDetails.Quantity, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetails.VATPercent, GoodsReceiptDetails.GrossPrice, GoodsReceiptDetails.Amount, GoodsReceiptDetails.VATAmount, GoodsReceiptDetails.GrossAmount, " + "\r\n";
            queryEdit = queryEdit + "                   Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.Remarks " + "\r\n";

            queryEdit = queryEdit + "   FROM            " + entityDetail + " INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)goodsReceiptTypeID + " AND " + entityDetail + "." + entityDetailID + " = GoodsReceiptDetails.VoucherDetailID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Commodities ON GoodsReceiptDetails.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";

            queryEdit = queryEdit + "   WHERE           (@VoucherID = 0 OR " + entityDetail + "." + entityID + " = @VoucherID) AND GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID " + "\r\n";

            querySort = "               ORDER BY        " + entityDetail + "." + entityDetailID;


            queryString = " IF @GoodsReceiptTypeID = " + (int)goodsReceiptTypeID;
            queryString = queryString + " BEGIN ";

            queryString = queryString + " IF (@GoodsReceiptID <= 0) " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       " + queryNew + "\r\n";
            queryString = queryString + "       " + querySort + "\r\n";
            queryString = queryString + "    END " + "\r\n";
            queryString = queryString + " ELSE " + "\r\n";

            queryString = queryString + "   IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           " + queryEdit + "\r\n";
            queryString = queryString + "           " + querySort + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            queryString = queryString + "   ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           " + queryNew + " AND " + entityDetail + "." + entityDetailID + " NOT IN (SELECT VoucherDetailID FROM GoodsReceiptDetails WHERE GoodsReceiptTypeID = " + (int)goodsReceiptTypeID + " AND GoodsReceiptID = @GoodsReceiptID) " + "\r\n";
            queryString = queryString + "           UNION ALL " + "\r\n";
            queryString = queryString + "           " + queryEdit + "\r\n";

            queryString = queryString + "           " + querySort + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            queryString = queryString + " END ";

            return queryString;
        }


        
        private void GoodsReceiptSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @GoodsReceiptTypeID int " + "\r\n";
            queryString = queryString + "       SELECT TOP 1 @GoodsReceiptTypeID = GoodsReceiptTypeID FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID " + "\r\n";

            queryString = queryString + "       " + this.GoodsReceiptSaveRelativeBuildSQL(GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice);
            queryString = queryString + "       " + this.GoodsReceiptSaveRelativeBuildSQL(GlobalEnums.GoodsReceiptTypeID.StockTransfer);


            queryString = queryString + "       EXEC UpdateWarehouseBalance @SaveRelativeOption, @EntityID, 0, 0, 0 ";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GoodsReceiptSaveRelative", queryString);

        }

        private string GoodsReceiptSaveRelativeBuildSQL(GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID)
        {
            string queryString;
            string entityDetail = goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ? "PurchaseInvoiceDetails" : goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer ? "StockTransferDetails" : "";
            string entityID = goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ? "PurchaseInvoiceID" : goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer ? "StockTransferID" : "";
            string entityDetailID = goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ? "PurchaseInvoiceDetailID" : goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer ? "StockTransferDetailID" : "";

            queryString = " IF @GoodsReceiptTypeID = " + (int)goodsReceiptTypeID + "\r\n";
            queryString = queryString + " BEGIN " + "\r\n";
            queryString = queryString + "       UPDATE          " + entityDetail + "\r\n";
            queryString = queryString + "       SET             " + entityDetail + ".QuantityReceipt = ROUND(" + entityDetail + ".QuantityReceipt + GoodsReceiptDetails.Quantity * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       " + entityDetail + " ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.VoucherDetailID = " + entityDetail + "." + entityDetailID + "\r\n";
            queryString = queryString + " END " + "\r\n";

            return queryString;

        }



        private void GoodsReceiptPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " DECLARE @GoodsReceiptTypeID Int            DECLARE @VoucherID Int              DECLARE @EntryDate Datetime " + "\r\n";
            queryArray[0] = queryArray[0] + " SELECT TOP 1 @GoodsReceiptTypeID = GoodsReceiptTypeID, @VoucherID = VoucherID, @EntryDate = EntryDate FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID " + "\r\n";
            queryArray[0] = queryArray[0] + " IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + ")" + "\r\n";
            queryArray[0] = queryArray[0] + "       SELECT TOP 1 @FoundEntity = 'Invoice Date: ' + CAST(EntryDate AS nvarchar) FROM PurchaseInvoices WHERE PurchaseInvoiceID = @VoucherID AND EntryDate > @EntryDate " + "\r\n";
            queryArray[0] = queryArray[0] + " IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + ")" + "\r\n";
            queryArray[0] = queryArray[0] + "       SELECT TOP 1 @FoundEntity = 'Transfer Date: ' + CAST(EntryDate AS nvarchar) FROM StockTransfers WHERE StockTransferID = @VoucherID AND EntryDate > @EntryDate " + "\r\n";

            queryArray[1] = "                 IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + ")" + "\r\n";
            queryArray[1] = queryArray[1] + "       SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityReceipt, 0) AS nvarchar) FROM PurchaseInvoiceDetails WHERE (ROUND(Quantity - QuantityReceipt, 0) < 0) " + "\r\n";
            queryArray[1] = queryArray[1] + " IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + ")" + "\r\n";
            queryArray[1] = queryArray[1] + "       SELECT TOP 1 @FoundEntity = 'Over Quantity: ' + CAST(ROUND(Quantity - QuantityReceipt, 0) AS nvarchar) FROM StockTransferDetails WHERE (ROUND(Quantity - QuantityReceipt, 0) < 0) " + "\r\n";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("GoodsReceiptPostSaveValidate", queryArray);
        }



        private void GoodsReceiptEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptDetailID FROM SalesInvoiceDetails WHERE GoodsReceiptDetailID IN (SELECT GoodsReceiptDetailID FROM GoodsReceiptDetails WHERE GoodsReceiptID = @EntityID) ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsReceiptDetailID FROM StockTransferDetails WHERE GoodsReceiptDetailID IN (SELECT GoodsReceiptDetailID FROM GoodsReceiptDetails WHERE GoodsReceiptID = @EntityID) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("GoodsReceiptEditable", queryArray);
        }


        private void GoodsReceiptInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsReceipts", "GoodsReceiptID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsReceipt));
            this.totalBikePortalsEntities.CreateTrigger("GoodsReceiptInitReference", simpleInitReference.CreateQuery());
        }

    }
}
