﻿using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.StockTasks
{
    public class WarehouseInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public WarehouseInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetWarehouseInvoiceIndexes();

            this.GetWarehouseInvoiceViewDetails();

            this.GetPendingGoodsReceipts();
            this.GetPendingGoodsReceiptDetails();

            this.GetPendingStockTransfers();
            this.GetPendingStockTransferDetails();

            this.WarehouseInvoiceSaveRelative();
            this.WarehouseInvoicePostSaveValidate();

            this.WarehouseInvoiceInitReference();

            this.WarehouseInvoiceSheet();
        }

        private void GetWarehouseInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseInvoices.WarehouseInvoiceID, CAST(WarehouseInvoices.EntryDate AS DATE) AS EntryDate, WarehouseInvoices.Reference, WarehouseInvoices.TransferOrderNo, WarehouseInvoices.TransferOrderCode, WarehouseInvoices.VATInvoiceNo, Locations.Code AS LocationCode, Warehouses.Name AS WarehouseName, ISNULL(SourceWarehouses.Name, N'MUA HÀNG') AS SourceWarehouseName, WarehouseInvoices.Description, WarehouseInvoices.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        WarehouseInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON WarehouseInvoices.EntryDate >= @FromDate AND WarehouseInvoices.EntryDate <= @ToDate AND WarehouseInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.WarehouseInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = WarehouseInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Warehouses ON WarehouseInvoices.WarehouseID = Warehouses.WarehouseID LEFT JOIN " + "\r\n";
            queryString = queryString + "                   Warehouses AS SourceWarehouses ON WarehouseInvoices.SourceWarehouseID = SourceWarehouses.WarehouseID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetWarehouseInvoiceIndexes", queryString);
        }

        private void GetWarehouseInvoiceViewDetails()
        {
            string queryString;

            queryString = " @WarehouseInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (NOT (SELECT MIN(GoodsReceiptDetailID) FROM WarehouseInvoiceDetails WHERE WarehouseInvoiceID = @WarehouseInvoiceID) IS NULL) " + "\r\n";
            queryString = queryString + "                   " + this.GetWarehouseInvoiceViewDetailSQL(true) + " INNER JOIN " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON WarehouseInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID INNER JOIN " + "\r\n";
            queryString = queryString + "                   PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "                   " + this.GetWarehouseInvoiceViewDetailSQL(false) + " LEFT JOIN " + "\r\n";
            queryString = queryString + "                   StockTransferDetails ON WarehouseInvoiceDetails.StockTransferDetailID = StockTransferDetails.StockTransferDetailID LEFT JOIN " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetWarehouseInvoiceViewDetails", queryString);
        }

        private string GetWarehouseInvoiceViewDetailSQL(bool isPurchase)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      WarehouseInvoiceDetails.WarehouseInvoiceDetailID, WarehouseInvoiceDetails.WarehouseInvoiceID, " + (isPurchase ? "PurchaseInvoices.VATInvoiceNo" : "NULL") + " AS VATInvoiceNo, WarehouseInvoiceDetails.GoodsReceiptID, WarehouseInvoiceDetails.GoodsReceiptDetailID, WarehouseInvoiceDetails.StockTransferID, WarehouseInvoiceDetails.StockTransferDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   WarehouseInvoiceDetails.Quantity, WarehouseInvoiceDetails.ListedPrice, WarehouseInvoiceDetails.DiscountPercent, WarehouseInvoiceDetails.UnitPrice, WarehouseInvoiceDetails.VATPercent, WarehouseInvoiceDetails.GrossPrice, WarehouseInvoiceDetails.Amount, WarehouseInvoiceDetails.VATAmount, WarehouseInvoiceDetails.GrossAmount, WarehouseInvoiceDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        WarehouseInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON WarehouseInvoiceDetails.WarehouseInvoiceID = @WarehouseInvoiceID AND WarehouseInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        private void GetPendingGoodsReceipts()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @LocationID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      DISTINCT GoodsReceiptDetails.WarehouseID, Warehouses.Name AS WarehouseName, Locations.Telephone AS WarehouseLocationTelephone, Locations.Facsimile AS WarehouseLocationFacsimile, Locations.Name AS WarehouseLocationName, Locations.Address AS WarehouseLocationAddress " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON GoodsReceipts.GoodsReceiptID = GoodsReceiptDetails.GoodsReceiptID AND GoodsReceiptDetails.WarehouseInvoiceID IS NULL AND GoodsReceiptDetails.EntryDate >= CONVERT(smalldatetime, '01/08/2019',103)  AND GoodsReceipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.GoodsReceipt + " AND AccessControls.AccessLevel >= 1)      " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON Warehouses.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "       ORDER BY    Warehouses.Name " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingGoodsReceipts", queryString);
        }

        private void GetPendingGoodsReceiptDetails()
        {
            string queryString;

            queryString = " @GoodsReceiptID Int, @AspUserID nvarchar(128), @LocationID Int, @WarehouseID Int, @CommodityTypeID Int, @FromDate DateTime, @ToDate DateTime, @WarehouseInvoiceID Int, @GoodsReceiptDetailIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@GoodsReceiptID <> 0) " + "\r\n"; //IF @GoodsReceiptID <> 0 THEN ALL OTHER Filter Parameters WILL BE NoNeeded. THIS CASE IS only USED TO MAKE ACCOUNTINVOICE AUTOMATICALLY FROM VEHICLEINVOICE
            queryString = queryString + "           " + this.GetPendingGoodsReceiptDetailsBuildSQL(true, false) + "\r\n";
            queryString = queryString + "       ELSE IF  (@CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.All + ") " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsReceiptDetailsBuildSQL(false, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsReceiptDetailsBuildSQL(false, true) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingGoodsReceiptDetails", queryString);
        }

        private string GetPendingGoodsReceiptDetailsBuildSQL(bool isGoodsReceiptID, bool isCommodityTypeID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@WarehouseInvoiceID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsReceiptDetailsBuildSQLA(isGoodsReceiptID, isCommodityTypeID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsReceiptDetailsBuildSQLA(isGoodsReceiptID, isCommodityTypeID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsReceiptDetailsBuildSQLA(bool isGoodsReceiptID, bool isCommodityTypeID, bool isWarehouseInvoiceID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsReceiptDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsReceiptDetailsBuildSQLB(isGoodsReceiptID, isCommodityTypeID, isWarehouseInvoiceID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingGoodsReceiptDetailsBuildSQLB(isGoodsReceiptID, isCommodityTypeID, isWarehouseInvoiceID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingGoodsReceiptDetailsBuildSQLB(bool isGoodsReceiptID, bool isCommodityTypeID, bool isWarehouseInvoiceID, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.EntryDate, PurchaseInvoices.VATInvoiceNo, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.Name AS WarehouseName, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.Quantity, GoodsReceiptDetails.UnitPrice AS ListedPrice, CAST(0 AS decimal(18, 5)) AS DiscountPercent, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetails.VATPercent, GoodsReceiptDetails.GrossPrice, GoodsReceiptDetails.Amount, GoodsReceiptDetails.VATAmount, GoodsReceiptDetails.GrossAmount, CAST(1 AS bit) AS IsSelected " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceiptDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON Commodities.CommodityCategoryID <> " + (int)GlobalEnums.CommodityCategoryID53 + " AND GoodsReceiptDetails.WarehouseID = @WarehouseID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + (isCommodityTypeID ? " AND GoodsReceiptDetails.CommodityTypeID = @CommodityTypeID" : "") + " AND GoodsReceiptDetails.GoodsReceiptID " + (isGoodsReceiptID ? " = @GoodsReceiptID " : " IN (SELECT GoodsReceiptID FROM GoodsReceipts WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND GoodsReceipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.GoodsReceipt + " AND AccessControls.AccessLevel >= 1))      AND (GoodsReceiptDetails.WarehouseInvoiceID IS NULL " + (isWarehouseInvoiceID ? " OR GoodsReceiptDetails.WarehouseInvoiceID = @WarehouseInvoiceID" : "") + ")" + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "")) + " AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 INNER JOIN " + "\r\n"; //AND LocationID = @LocationID
            queryString = queryString + "                   Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID INNER JOIN" + "\r\n";
            queryString = queryString + "                   PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }



        private void GetPendingStockTransfers()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @LocationID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      DISTINCT StockTransferDetails.WarehouseID AS SourceWarehouseID, SourceWarehouses.Name AS SourceWarehouseName, StockTransfers.WarehouseID, Warehouses.Name AS WarehouseName, Locations.Telephone AS WarehouseLocationTelephone, Locations.Facsimile AS WarehouseLocationFacsimile, Locations.Name AS WarehouseLocationName, Locations.Address AS WarehouseLocationAddress " + "\r\n";
            queryString = queryString + "       FROM        StockTransfers " + "\r\n";
            queryString = queryString + "                   INNER JOIN StockTransferDetails ON StockTransfers.StockTransferID = StockTransferDetails.StockTransferID AND StockTransferDetails.WarehouseInvoiceID IS NULL AND StockTransferDetails.EntryDate >= CONVERT(smalldatetime, '01/08/2019',103)  AND StockTransfers.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID IN (" + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.VehicleTransfer + ", " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PartTransfer + ") AND AccessControls.AccessLevel >= 1)      " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON StockTransfers.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON Warehouses.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses AS SourceWarehouses ON StockTransferDetails.WarehouseID = SourceWarehouses.WarehouseID " + "\r\n";
            queryString = queryString + "       ORDER BY    SourceWarehouses.Name, Warehouses.Name " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingStockTransfers", queryString);
        }

        private void GetPendingStockTransferDetails()
        {
            string queryString;

            queryString = " @StockTransferID Int, @AspUserID nvarchar(128), @LocationID Int, @SourceWarehouseID Int, @WarehouseID Int, @StockTransferTypeID Int, @FromDate DateTime, @ToDate DateTime, @WarehouseInvoiceID Int, @StockTransferDetailIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @PendingStockTransferDetails TABLE (StockTransferID int NOT NULL, StockTransferDetailID int NOT NULL, EntryDate datetime NOT NULL, CommodityID int NOT NULL, CommodityCode nvarchar(50) NOT NULL, CommodityName nvarchar(250) NOT NULL, CommodityTypeID int  NOT NULL, WarehouseName nvarchar(60) NOT NULL, ChassisCode nvarchar(60) NULL, EngineCode nvarchar(60) NULL, ColorCode nvarchar(60) NULL, Quantity decimal(18, 2) NOT NULL, ListedPrice decimal(18, 2) NOT NULL, DiscountPercent decimal(18, 5) NOT NULL, UnitPrice decimal(18, 2) NOT NULL, VATPercent decimal(18, 2) NOT NULL, GrossPrice decimal(18, 2) NOT NULL) " + "\r\n";

            queryString = queryString + "       IF  (@StockTransferID <> 0) " + "\r\n"; //IF @StockTransferID <> 0 THEN ALL OTHER Filter Parameters WILL BE NoNeeded. THIS CASE IS only USED TO MAKE ACCOUNTINVOICE AUTOMATICALLY FROM VEHICLEINVOICE
            queryString = queryString + "           " + this.GetPendingStockTransferDetailsBuildSQL(true, false) + "\r\n";
            queryString = queryString + "       ELSE IF  (@StockTransferTypeID = " + (int)GlobalEnums.StockTransferTypeID.AllStockTransfer + ") " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransferDetailsBuildSQL(false, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransferDetailsBuildSQL(false, true) + "\r\n";



            queryString = queryString + "       UPDATE PendingStockTransferDetails SET PendingStockTransferDetails.ListedPrice = WarehouseBalancePrice.UnitPrice, PendingStockTransferDetails.UnitPrice = ROUND(WarehouseBalancePrice.UnitPrice, 0), PendingStockTransferDetails.GrossPrice = ROUND(WarehouseBalancePrice.UnitPrice * (1 + PendingStockTransferDetails.VATPercent / 100), 0) FROM @PendingStockTransferDetails PendingStockTransferDetails INNER JOIN WarehouseBalancePrice ON PendingStockTransferDetails.CommodityTypeID <> 1 AND PendingStockTransferDetails.CommodityID = WarehouseBalancePrice.CommodityID AND dbo.EOMONTHTIME(PendingStockTransferDetails.EntryDate, 9999) = WarehouseBalancePrice.EntryDate " + "\r\n";

            queryString = queryString + "       SELECT StockTransferID, StockTransferDetailID, CommodityID, CommodityCode, CommodityName, CommodityTypeID, WarehouseName, ChassisCode, EngineCode, ColorCode, Quantity, ListedPrice, DiscountPercent, UnitPrice, VATPercent, GrossPrice, ROUND(Quantity * UnitPrice, 0) AS Amount, ROUND(ROUND(Quantity * GrossPrice, 0) - ROUND(Quantity * UnitPrice, 0), 0) AS VATAmount, ROUND(Quantity * GrossPrice, 0) AS GrossAmount, CAST(1 AS bit) AS IsSelected FROM @PendingStockTransferDetails " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingStockTransferDetails", queryString);
        }

        private string GetPendingStockTransferDetailsBuildSQL(bool isStockTransferID, bool isStockTransferTypeID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@WarehouseInvoiceID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransferDetailsBuildSQLA(isStockTransferID, isStockTransferTypeID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransferDetailsBuildSQLA(isStockTransferID, isStockTransferTypeID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingStockTransferDetailsBuildSQLA(bool isStockTransferID, bool isStockTransferTypeID, bool isWarehouseInvoiceID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@StockTransferDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransferDetailsBuildSQLB(isStockTransferID, isStockTransferTypeID, isWarehouseInvoiceID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingStockTransferDetailsBuildSQLB(isStockTransferID, isStockTransferTypeID, isWarehouseInvoiceID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }
        /// <summary>
        /// KTRA: SAVE UPDATE -- IsFinished KTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinished
        /// </summary>
        /// <param name="isStockTransferTypeID"></param>
        /// <param name="isWarehouseInvoiceID"></param>
        /// <param name="isStockTransferDetailIDs"></param>
        /// <returns></returns>
        private string GetPendingStockTransferDetailsBuildSQLB(bool isStockTransferID, bool isStockTransferTypeID, bool isWarehouseInvoiceID, bool isStockTransferDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO @PendingStockTransferDetails(StockTransferID, StockTransferDetailID, EntryDate, CommodityID, CommodityCode, CommodityName, CommodityTypeID, WarehouseName, ChassisCode, EngineCode, ColorCode, Quantity, ListedPrice, DiscountPercent, UnitPrice, VATPercent, GrossPrice) " + "\r\n";

            queryString = queryString + "       SELECT      StockTransferDetails.StockTransferID, StockTransferDetails.StockTransferDetailID, StockTransferDetails.EntryDate, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.Name AS WarehouseName, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   StockTransferDetails.Quantity, ISNULL(GoodsReceiptDetails.UnitPrice, 0) AS ListedPrice, CAST(0 AS decimal(18, 5)) AS DiscountPercent, ISNULL(GoodsReceiptDetails.UnitPrice, 0) AS UnitPrice, ISNULL(GoodsReceiptDetails.VATPercent, CommodityCategories.VATPercent) AS VATPercent, ISNULL(GoodsReceiptDetails.GrossPrice, 0) AS GrossPrice " + "\r\n";
            queryString = queryString + "       FROM        StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON Commodities.CommodityCategoryID <> " + (int)GlobalEnums.CommodityCategoryID53 + " AND StockTransferDetails.WarehouseID = @SourceWarehouseID AND StockTransferDetails.StockTransferID " + (isStockTransferID ? " = @StockTransferID " : " IN (SELECT StockTransferID FROM StockTransfers WHERE EntryDate >= @FromDate AND EntryDate <= @ToDate AND WarehouseID = @WarehouseID " + (isStockTransferTypeID ? " AND StockTransferTypeID = @StockTransferTypeID" : "") + " AND StockTransfers.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID IN (" + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.VehicleTransfer + ", " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PartTransfer + ") AND AccessControls.AccessLevel >= 1))      AND (StockTransferDetails.WarehouseInvoiceID IS NULL " + (isWarehouseInvoiceID ? " OR StockTransferDetails.WarehouseInvoiceID = @WarehouseInvoiceID" : "") + ")" + (isStockTransferDetailIDs ? " AND StockTransferDetails.StockTransferDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@StockTransferDetailIDs))" : "")) + " AND StockTransferDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 INNER JOIN " + "\r\n"; //AND LocationID = @LocationID
            queryString = queryString + "                   CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Warehouses ON StockTransferDetails.WarehouseID = Warehouses.WarehouseID LEFT JOIN " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        private void WarehouseInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @GoodsReceiptDetailID int = (SELECT MIN(GoodsReceiptDetailID) FROM WarehouseInvoiceDetails WHERE WarehouseInvoiceID = @EntityID) " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF (NOT @GoodsReceiptDetailID IS NULL) " + "\r\n";
            queryString = queryString + "                   UPDATE      GoodsReceiptDetails" + "\r\n";
            queryString = queryString + "                   SET         GoodsReceiptDetails.WarehouseInvoiceID = WarehouseInvoiceDetails.WarehouseInvoiceID " + "\r\n";
            queryString = queryString + "                   FROM        GoodsReceiptDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                               WarehouseInvoiceDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @EntityID AND GoodsReceiptDetails.GoodsReceiptDetailID = WarehouseInvoiceDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   UPDATE      StockTransferDetails" + "\r\n";
            queryString = queryString + "                   SET         StockTransferDetails.WarehouseInvoiceID = WarehouseInvoiceDetails.WarehouseInvoiceID " + "\r\n";
            queryString = queryString + "                   FROM        StockTransferDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                               WarehouseInvoiceDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @EntityID AND StockTransferDetails.StockTransferDetailID = WarehouseInvoiceDetails.StockTransferDetailID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF (NOT @GoodsReceiptDetailID IS NULL) " + "\r\n";
            queryString = queryString + "                   UPDATE      GoodsReceiptDetails" + "\r\n";
            queryString = queryString + "                   SET         WarehouseInvoiceID = NULL " + "\r\n";
            queryString = queryString + "                   WHERE       WarehouseInvoiceID = @EntityID " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   UPDATE      StockTransferDetails" + "\r\n";
            queryString = queryString + "                   SET         WarehouseInvoiceID = NULL " + "\r\n";
            queryString = queryString + "                   WHERE       WarehouseInvoiceID = @EntityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("WarehouseInvoiceSaveRelative", queryString);
        }

        private void WarehouseInvoicePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Phiếu nhập kho chưa hoàn tất, hoặc ngày hóa đơn không hợp lệ. Ngày nhập kho: ' + CAST(GoodsReceiptDetails.EntryDate AS nvarchar) FROM WarehouseInvoiceDetails INNER JOIN GoodsReceiptDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @EntityID AND WarehouseInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID AND (WarehouseInvoiceDetails.EntryDate < GoodsReceiptDetails.EntryDate  OR CAST(WarehouseInvoiceDetails.EntryDate AS DATE) <> CAST(GoodsReceiptDetails.EntryDate AS DATE))";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Phiếu chuyển kho chưa hoàn tất, hoặc ngày hóa đơn không hợp lệ. Ngày chuyển kho: ' + CAST(StockTransferDetails.EntryDate AS nvarchar) FROM WarehouseInvoiceDetails INNER JOIN StockTransferDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @EntityID AND WarehouseInvoiceDetails.StockTransferDetailID = StockTransferDetails.StockTransferDetailID AND (WarehouseInvoiceDetails.EntryDate < StockTransferDetails.EntryDate  OR CAST(WarehouseInvoiceDetails.EntryDate AS DATE) <> CAST(StockTransferDetails.EntryDate AS DATE))";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("WarehouseInvoicePostSaveValidate", queryArray);
        }



        private void WarehouseInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("WarehouseInvoices", "WarehouseInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.WarehouseInvoice));
            this.totalBikePortalsEntities.CreateTrigger("WarehouseInvoiceInitReference", simpleInitReference.CreateQuery());
        }


        private void WarehouseInvoiceSheet()
        {
            string queryString = " @WarehouseInvoiceID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalWarehouseInvoiceID int    SET @LocalWarehouseInvoiceID = @WarehouseInvoiceID                  DECLARE @PurchaseVATInvoiceNo nvarchar(300) " + "\r\n";

            queryString = queryString + "       IF (NOT (SELECT MIN(GoodsReceiptDetailID) FROM WarehouseInvoiceDetails WHERE WarehouseInvoiceID = @WarehouseInvoiceID) IS NULL) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SELECT  @PurchaseVATInvoiceNo = STUFF((SELECT ', ' + VATInvoiceNo FROM (SELECT DISTINCT PurchaseInvoices.VATInvoiceNo FROM WarehouseInvoiceDetails INNER JOIN GoodsReceiptDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @LocalWarehouseInvoiceID AND WarehouseInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID INNER JOIN PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID) DISTINCTVATInvoiceNo FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               " + this.WarehouseInvoiceSheetSQL(true) + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.WarehouseInvoiceSheetSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("WarehouseInvoiceSheet", queryString);
        }

        private string WarehouseInvoiceSheetSQL(bool GRvsST)
        {
            string queryString = " " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT          WarehouseInvoices.WarehouseInvoiceID, WarehouseInvoices.EntryDate, WarehouseInvoices.Reference, WarehouseInvoices.TransferOrderNo, WarehouseInvoices.TransferOrderCode, WarehouseInvoices.VATInvoiceNo, WarehouseInvoices.VATInvoiceDate, WarehouseInvoices.VATInvoiceSeries, @PurchaseVATInvoiceNo AS PurchaseVATInvoiceNo, " + "\r\n";
            queryString = queryString + "                       Warehouses.WarehouseID, Warehouses.Name AS WarehouseName, WarehouseInvoices.SourceWarehouseID, SourceWarehouses.Name AS SourceWarehouseName, " + "\r\n";
            queryString = queryString + "                       " + (GRvsST ? "10" : "MINGoodsReceiptDetails.MINStockTransferTypeID") + " AS MINStockTransferTypeID, WarehouseInvoiceCollections.GoodsReceiptDetailID, WarehouseInvoiceCollections.StockTransferDetailID, ISNULL(WarehouseInvoiceCollections.IsBonus, CAST(0 AS bit)) AS IsBonus, Commodities.CommodityID, Commodities.CommodityTypeID, Commodities.Code, Commodities.Name  + ' ' + ISNULL('(' + WarehouseInvoiceCollections.Remarks + ')', '') AS Name, Commodities.SalesUnit, WarehouseInvoiceCollections.LineDescription AS LineDescription, WarehouseInvoiceCollections.ChassisCode, WarehouseInvoiceCollections.EngineCode, WarehouseInvoiceCollections.ColorCode, WarehouseInvoiceCollections.ColorCodeName, " + "\r\n";
            queryString = queryString + "                       WarehouseInvoiceCollections.Quantity, WarehouseInvoiceCollections.UnitPrice, WarehouseInvoiceCollections.VATPercent, WarehouseInvoiceCollections.Amount, WarehouseInvoiceCollections.VATAmount, WarehouseInvoiceCollections.GrossAmount, " + "\r\n";
            queryString = queryString + "                       WarehouseInvoices.TotalQuantity, WarehouseInvoices.TotalAmount, WarehouseInvoices.TotalVATAmount, WarehouseInvoices.TotalGrossAmount, dbo.SayVND(WarehouseInvoices.TotalGrossAmount) AS TotalGrossAmountInWords, WarehouseInvoices.Description, " + "\r\n";
            queryString = queryString + "                       Locations.OfficialName AS LocationOfficialName, Locations.Address AS LocationAddress, Locations.Taxcode AS LocationTaxcode, Locations.Telephone AS LocationTelephone, Locations.Facsimile AS LocationFacsimile, " + "\r\n";
            queryString = queryString + "                       SourceLocations.OfficialName AS SourceLocationOfficialName, SourceLocations.Address AS SourceLocationAddress, SourceLocations.Taxcode AS SourceLocationTaxcode, SourceLocations.Telephone AS SourceLocationTelephone, SourceLocations.Facsimile AS SourceLocationFacsimile " + "\r\n";

            if (GRvsST)
                queryString = queryString + "   FROM            (SELECT WarehouseInvoiceDetails.WarehouseInvoiceID, WarehouseInvoiceDetails.GoodsReceiptDetailID, WarehouseInvoiceDetails.StockTransferDetailID, WarehouseInvoiceDetails.CommodityID, N'' AS LineDescription, WarehouseInvoiceDetails.Quantity, WarehouseInvoiceDetails.UnitPrice, WarehouseInvoiceDetails.Amount, WarehouseInvoiceDetails.VATPercent, WarehouseInvoiceDetails.VATAmount, WarehouseInvoiceDetails.GrossAmount, WarehouseInvoiceDetails.Remarks, CAST(0 AS bit) AS IsBonus, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, ISNULL(ColorCodes.Name, GoodsReceiptDetails.ColorCode) AS ColorCodeName FROM WarehouseInvoiceDetails INNER JOIN GoodsReceiptDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @LocalWarehouseInvoiceID AND WarehouseInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID LEFT JOIN ColorCodes ON GoodsReceiptDetails.CommodityID = ColorCodes.CommodityID AND GoodsReceiptDetails.ColorCode = ColorCodes.Code) AS WarehouseInvoiceCollections " + "\r\n";

            if (!GRvsST)
                queryString = queryString + "   FROM            (SELECT WarehouseInvoiceDetails.WarehouseInvoiceID, WarehouseInvoiceDetails.GoodsReceiptDetailID, WarehouseInvoiceDetails.StockTransferDetailID, WarehouseInvoiceDetails.CommodityID, N'' AS LineDescription, WarehouseInvoiceDetails.Quantity, WarehouseInvoiceDetails.UnitPrice, WarehouseInvoiceDetails.Amount, WarehouseInvoiceDetails.VATPercent, WarehouseInvoiceDetails.VATAmount, WarehouseInvoiceDetails.GrossAmount, StockTransferDetails.Remarks, CAST(0 AS bit) AS IsBonus, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, ISNULL(ColorCodes.Name, GoodsReceiptDetails.ColorCode) AS ColorCodeName FROM WarehouseInvoiceDetails INNER JOIN StockTransferDetails ON WarehouseInvoiceDetails.WarehouseInvoiceID = @LocalWarehouseInvoiceID AND WarehouseInvoiceDetails.StockTransferDetailID = StockTransferDetails.StockTransferDetailID LEFT JOIN GoodsReceiptDetails ON StockTransferDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID LEFT JOIN ColorCodes ON GoodsReceiptDetails.CommodityID = ColorCodes.CommodityID AND GoodsReceiptDetails.ColorCode = ColorCodes.Code) AS WarehouseInvoiceCollections " + "\r\n";

            queryString = queryString + "                       INNER JOIN WarehouseInvoices ON WarehouseInvoiceCollections.WarehouseInvoiceID = WarehouseInvoices.WarehouseInvoiceID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON WarehouseInvoices.WarehouseID = Warehouses.WarehouseID " + "\r\n";

            if (!GRvsST)
                queryString = queryString + "                   INNER JOIN (SELECT MIN(StockTransfers.StockTransferTypeID) AS MINStockTransferTypeID FROM StockTransfers INNER JOIN StockTransferDetails ON StockTransferDetails.WarehouseInvoiceID = @LocalWarehouseInvoiceID AND StockTransfers.StockTransferID = StockTransferDetails.StockTransferID) MINGoodsReceiptDetails ON 1 = 1 " + "\r\n";

            queryString = queryString + "                       INNER JOIN Locations ON Warehouses.LocationID = Locations.LocationID " + "\r\n";
            
            queryString = queryString + "                       LEFT JOIN Warehouses AS SourceWarehouses ON WarehouseInvoices.SourceWarehouseID = SourceWarehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN Locations SourceLocations ON SourceWarehouses.LocationID = SourceLocations.LocationID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN Commodities ON WarehouseInvoiceCollections.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }
    }
}
