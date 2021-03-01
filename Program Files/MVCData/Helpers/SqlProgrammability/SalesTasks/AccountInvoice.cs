using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class AccountInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public AccountInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetAccountInvoiceIndexes();

            this.GetAccountInvoiceViewDetails();
            this.GetPendingSalesInvoices();

            this.AccountInvoiceSaveRelative();
            this.AccountInvoicePostSaveValidate();

            this.AccountInvoiceApproved();
            this.AccountInvoiceEditable();

            this.AccountInvoiceToggleApproved();

            this.UpdateAccountInvoiceApi();
            this.ClearAccountInvoiceApi();

            this.AccountInvoiceInitReference();

            this.AccountInvoiceSheet();
        }

        private void GetAccountInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoices.AccountInvoiceID, CAST(AccountInvoices.EntryDate AS DATE) AS EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, AccountInvoices.Description, AccountInvoices.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        AccountInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON AccountInvoices.EntryDate >= @FromDate AND AccountInvoices.EntryDate <= @ToDate AND AccountInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.AccountInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = AccountInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetAccountInvoiceIndexes", queryString);
        }

        private void GetAccountInvoiceViewDetails()
        {
            string queryString;

            queryString = " @AccountInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.AccountInvoiceID, AccountInvoiceDetails.SalesInvoiceDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails.Quantity, SalesInvoiceDetails.ListedPrice, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, SalesInvoiceDetails.IsBonus, SalesInvoiceDetails.IsWarrantyClaim, AccountInvoiceDetails.Remarks" + "\r\n";
            queryString = queryString + "       FROM        AccountInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND AccountInvoiceDetails.SalesInvoiceDetailID = SalesInvoiceDetails.SalesInvoiceDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON SalesInvoiceDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetAccountInvoiceViewDetails", queryString);
        }

        private void GetPendingSalesInvoices()
        {
            string queryString;

            queryString = " @SalesInvoiceID Int, @AspUserID nvarchar(128), @LocationID Int, @SalesInvoiceTypeID Int, @FromDate DateTime, @ToDate DateTime, @AccountInvoiceID Int, @SalesInvoiceDetailIDs varchar(3999) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@SalesInvoiceID <> 0) " + "\r\n"; //IF @SalesInvoiceID <> 0 THEN ALL OTHER Filter Parameters WILL BE NoNeeded. THIS CASE IS only USED TO MAKE ACCOUNTINVOICE AUTOMATICALLY FROM VEHICLEINVOICE
            queryString = queryString + "           " + this.GetPendingSalesInvoicesBuildSQL(true, false) + "\r\n";
            queryString = queryString + "       ELSE IF  (@SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.AllInvoice + ") " + "\r\n";
            queryString = queryString + "           " + this.GetPendingSalesInvoicesBuildSQL(false, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingSalesInvoicesBuildSQL(false, true) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPendingSalesInvoices", queryString);
        }

        private string GetPendingSalesInvoicesBuildSQL(bool isSalesInvoiceID, bool isSalesInvoiceTypeID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@AccountInvoiceID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingSalesInvoicesBuildSQLA(isSalesInvoiceID, isSalesInvoiceTypeID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingSalesInvoicesBuildSQLA(isSalesInvoiceID, isSalesInvoiceTypeID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingSalesInvoicesBuildSQLA(bool isSalesInvoiceID, bool isSalesInvoiceTypeID, bool isAccountInvoiceID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@SalesInvoiceDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingSalesInvoicesBuildSQLB(isSalesInvoiceID, isSalesInvoiceTypeID, isAccountInvoiceID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingSalesInvoicesBuildSQLB(isSalesInvoiceID, isSalesInvoiceTypeID, isAccountInvoiceID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }
        /// <summary>
        /// KTRA: SAVE UPDATE -- IsFinished KTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinishedKTRA: SAVE UPDATE -- IsFinished
        /// </summary>
        /// <param name="isSalesInvoiceTypeID"></param>
        /// <param name="isAccountInvoiceID"></param>
        /// <param name="isSalesInvoiceDetailIDs"></param>
        /// <returns></returns>
        private string GetPendingSalesInvoicesBuildSQLB(bool isSalesInvoiceID, bool isSalesInvoiceTypeID, bool isAccountInvoiceID, bool isSalesInvoiceDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.SalesInvoiceDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Customers.Name AS CustomerName, Customers.AddressNo, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails.Quantity, SalesInvoiceDetails.ListedPrice, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, SalesInvoiceDetails.IsBonus, SalesInvoiceDetails.IsWarrantyClaim, CAST(1 AS bit) AS IsSelected " + "\r\n";
            queryString = queryString + "       FROM        SalesInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON Commodities.CommodityCategoryID <> " + (int)GlobalEnums.CommodityCategoryID53 + " AND SalesInvoiceDetails.IsFinished = 1 AND SalesInvoiceDetails.SalesInvoiceID " + (isSalesInvoiceID ? " IN (SELECT @SalesInvoiceID AS SalesInvoiceID UNION ALL SELECT SalesInvoiceID FROM SalesInvoices WHERE (ServiceInvoiceID = @SalesInvoiceID)) " : " IN (SELECT SalesInvoices.SalesInvoiceID FROM                  SalesInvoices LEFT JOIN SalesInvoices AS ServiceInvoices ON SalesInvoices.ServiceInvoiceID = ServiceInvoices.SalesInvoiceID             WHERE SalesInvoices.EntryDate >= @FromDate AND SalesInvoices.EntryDate <= @ToDate AND SalesInvoices.LocationID = @LocationID " + (isSalesInvoiceTypeID ? " AND SalesInvoices.SalesInvoiceTypeID = @SalesInvoiceTypeID" : "") + " AND ( (SalesInvoices.SalesInvoiceTypeID <> " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + " AND SalesInvoices.ServiceInvoiceID IS NULL) OR SalesInvoices.IsFinished = 1 OR ServiceInvoices.IsFinished = 1) AND SalesInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID IN (" + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.VehiclesInvoice + ", " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PartsInvoice + ", " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.ServicesInvoice + ") AND AccessControls.AccessLevel >= 1))      AND (SalesInvoiceDetails.AccountInvoiceID IS NULL " + (isAccountInvoiceID ? " OR SalesInvoiceDetails.AccountInvoiceID = @AccountInvoiceID" : "") + ")" + (isSalesInvoiceDetailIDs ? " AND SalesInvoiceDetails.SalesInvoiceDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@SalesInvoiceDetailIDs))" : "")) + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON SalesInvoiceDetails.CustomerID = Customers.CustomerID LEFT JOIN " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }


        private void AccountInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           UPDATE      SalesInvoiceDetails" + "\r\n";
            queryString = queryString + "           SET         SalesInvoiceDetails.AccountInvoiceID = AccountInvoiceDetails.AccountInvoiceID " + "\r\n";
            queryString = queryString + "           FROM        SalesInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                       AccountInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND SalesInvoiceDetails.SalesInvoiceDetailID = AccountInvoiceDetails.SalesInvoiceDetailID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           UPDATE      SalesInvoiceDetails" + "\r\n";
            queryString = queryString + "           SET         AccountInvoiceID = NULL " + "\r\n";
            queryString = queryString + "           WHERE       AccountInvoiceID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("AccountInvoiceSaveRelative", queryString);
        }

        private void AccountInvoicePostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Phiếu bán hàng chưa hoàn tất, hoặc ngày hóa đơn không hợp lệ. Ngày bán hàng: ' + CAST(SalesInvoiceDetails.EntryDate AS nvarchar) FROM AccountInvoiceDetails INNER JOIN SalesInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.SalesInvoiceDetailID = SalesInvoiceDetails.SalesInvoiceDetailID AND (SalesInvoiceDetails.IsFinished = 0 OR AccountInvoiceDetails.EntryDate < SalesInvoiceDetails.EntryDate OR CAST(AccountInvoiceDetails.EntryDate AS DATE) <> CAST(SalesInvoiceDetails.EntryDate AS DATE)) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("AccountInvoicePostSaveValidate", queryArray);
        }

        private void AccountInvoiceApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = AccountInvoiceID FROM AccountInvoices WHERE AccountInvoiceID = @EntityID AND Approved = 1";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("AccountInvoiceApproved", queryArray);
        }

        private void AccountInvoiceEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = AccountInvoiceID FROM AccountInvoices WHERE AccountInvoiceID = @EntityID AND NOT ResponedMessage IS NULL ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("AccountInvoiceEditable", queryArray);
        }


        private void AccountInvoiceToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      AccountInvoices  SET Approved = @Approved, ApprovedDate = GetDate() WHERE AccountInvoiceID = @EntityID AND Approved = ~@Approved" + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("AccountInvoiceToggleApproved", queryString);
        }


        private void UpdateAccountInvoiceApi()
        {
            string queryString = " @AccountInvoiceID int, @ApiSerialID int, @ApiSerialString nvarchar(30), @ResponedMessage nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           UPDATE          AccountInvoices " + "\r\n";
            queryString = queryString + "           SET             ApiSerialID = @ApiSerialID, ApiSerialString = @ApiSerialString, ResponedMessage = @ResponedMessage " + "\r\n";
            queryString = queryString + "           WHERE           AccountInvoiceID = @AccountInvoiceID AND Approved = 1" + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE         @msg NVARCHAR(300); " + "\r\n";
            queryString = queryString + "                   SET             @msg = N'Lỗi cập nhật database. Vui lòng kiểm tra lại trước khi tiếp tục' ; " + "\r\n";
            queryString = queryString + "                   THROW           61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("UpdateAccountInvoiceApi", queryString);
        }

        private void ClearAccountInvoiceApi()
        {
            string queryString = " @AccountInvoiceID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           UPDATE          AccountInvoices " + "\r\n";
            queryString = queryString + "           SET             ApiSerialID = NULL, ApiSerialString = NULL, ResponedMessage = NULL " + "\r\n";
            queryString = queryString + "           WHERE           AccountInvoiceID = @AccountInvoiceID AND Approved = 1" + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE         @msg NVARCHAR(300); " + "\r\n";
            queryString = queryString + "                   SET             @msg = N'Lỗi cập nhật database. Vui lòng kiểm tra lại trước khi tiếp tục' ; " + "\r\n";
            queryString = queryString + "                   THROW           61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("ClearAccountInvoiceApi", queryString);
        }


        private void AccountInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("AccountInvoices", "AccountInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.AccountInvoice));
            this.totalBikePortalsEntities.CreateTrigger("AccountInvoiceInitReference", simpleInitReference.CreateQuery());
        }


        private void AccountInvoiceSheet()
        {
            string queryString = " @AccountInvoiceID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalAccountInvoiceID int    SET @LocalAccountInvoiceID = @AccountInvoiceID" + "\r\n";

            queryString = queryString + "       SELECT          AccountInvoices.AccountInvoiceID, AccountInvoices.EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, AccountInvoices.VATInvoiceDate, AccountInvoices.VATInvoiceSeries, " + "\r\n";
            queryString = queryString + "                       Customers.CustomerID, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.VATCode, Customers.AddressNo, Customers.Telephone, EntireTerritories.EntireName AS EntireTerritoryEntireName, AccountInvoices.PaymentTermID, PaymentTerms.Name AS PaymentTermName, " + "\r\n";
            queryString = queryString + "                       MINGoodsReceiptDetails.MINSalesInvoiceTypeID, AccountInvoiceCollections.SalesInvoiceDetailID, ISNULL(AccountInvoiceCollections.IsBonus, CAST(0 AS bit)) AS IsBonus, Commodities.CommodityID, Commodities.Code, Commodities.OfficialName + ' ' + ISNULL('(' + AccountInvoiceCollections.Remarks + ')', '') AS Name, Commodities.SalesUnit, AccountInvoiceCollections.LineDescription AS LineDescription, AccountInvoiceCollections.ChassisCode, AccountInvoiceCollections.EngineCode, AccountInvoiceCollections.ColorCode, AccountInvoiceCollections.ColorCodeName, " + "\r\n";
            queryString = queryString + "                       AccountInvoiceCollections.Quantity, AccountInvoiceCollections.UnitPrice, AccountInvoiceCollections.VATPercent, AccountInvoiceCollections.Amount, AccountInvoiceCollections.VATAmount, AccountInvoiceCollections.GrossAmount, " + "\r\n";
            queryString = queryString + "                       AccountInvoices.TotalQuantity, AccountInvoices.TotalAmount, AccountInvoices.TotalVATAmount, AccountInvoices.TotalGrossAmount, dbo.SayVND(AccountInvoices.TotalGrossAmount) AS TotalGrossAmountInWords, AccountInvoices.Description, " + "\r\n";
            queryString = queryString + "                       Locations.OfficialName AS LocationOfficialName, Locations.Address AS LocationAddress, Locations.Taxcode AS LocationTaxcode, Locations.Telephone AS LocationTelephone, Locations.Facsimile AS LocationFacsimile, AccountInvoices.Approved " + "\r\n";

            queryString = queryString + "       FROM            (SELECT SalesInvoiceDetails.AccountInvoiceID, SalesInvoiceDetails.SalesInvoiceDetailID, SalesInvoiceDetails.CommodityID, N'' AS LineDescription, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, SalesInvoiceDetails.Remarks, SalesInvoiceDetails.IsBonus, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, ISNULL(ColorCodes.Name, GoodsReceiptDetails.ColorCode) AS ColorCodeName FROM SalesInvoiceDetails LEFT JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID LEFT JOIN ColorCodes ON GoodsReceiptDetails.CommodityID = ColorCodes.CommodityID AND GoodsReceiptDetails.ColorCode = ColorCodes.Code WHERE SalesInvoiceDetails.AccountInvoiceID = @LocalAccountInvoiceID) AS AccountInvoiceCollections " + "\r\n";

            queryString = queryString + "                       INNER JOIN AccountInvoices ON AccountInvoiceCollections.AccountInvoiceID = AccountInvoices.AccountInvoiceID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN PaymentTerms ON AccountInvoices.PaymentTermID = PaymentTerms.PaymentTermID  " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT MIN(SalesInvoiceTypeID) AS MINSalesInvoiceTypeID FROM SalesInvoiceDetails WHERE AccountInvoiceID = @LocalAccountInvoiceID) MINGoodsReceiptDetails ON 1 = 1 " + "\r\n";

            queryString = queryString + "                       INNER JOIN Locations ON AccountInvoices.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "                       LEFT JOIN Commodities ON AccountInvoiceCollections.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("AccountInvoiceSheet", queryString);
        }
    }
}
