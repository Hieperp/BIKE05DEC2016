using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    class AccountInvoice
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

            this.AccountInvoiceInitReference();
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
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
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
            queryString = queryString + "                   Commodities ON SalesInvoiceDetails.SalesInvoiceID " + (isSalesInvoiceID ? " IN (SELECT @SalesInvoiceID AS SalesInvoiceID UNION ALL SELECT SalesInvoiceID FROM SalesInvoices WHERE (ServiceInvoiceID = @SalesInvoiceID)) " : " IN (SELECT SalesInvoices.SalesInvoiceID FROM                  SalesInvoices LEFT JOIN SalesInvoices AS ServiceInvoices ON SalesInvoices.ServiceInvoiceID = ServiceInvoices.SalesInvoiceID             WHERE SalesInvoices.EntryDate >= @FromDate AND SalesInvoices.EntryDate <= @ToDate AND SalesInvoices.LocationID = @LocationID " + (isSalesInvoiceTypeID ? " AND SalesInvoices.SalesInvoiceTypeID = @SalesInvoiceTypeID" : "") + " AND ( (SalesInvoices.SalesInvoiceTypeID <> " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + " AND SalesInvoices.ServiceInvoiceID IS NULL) OR SalesInvoices.IsFinished = 1 OR ServiceInvoices.IsFinished = 1) AND SalesInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID IN (" + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.VehiclesInvoice + ", " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PartsInvoice + ", " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.ServicesInvoice + ") AND AccessControls.AccessLevel = 2))      AND (SalesInvoiceDetails.AccountInvoiceID IS NULL " + (isAccountInvoiceID ? " OR SalesInvoiceDetails.AccountInvoiceID = @AccountInvoiceID" : "") + ")" + (isSalesInvoiceDetailIDs ? " AND SalesInvoiceDetails.SalesInvoiceDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@SalesInvoiceDetailIDs))" : "")) + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID AND Commodities.IsRegularCheckUps = 0 INNER JOIN " + "\r\n";
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

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày bán hàng: ' + CAST(SalesInvoiceDetails.EntryDate AS nvarchar) FROM AccountInvoiceDetails INNER JOIN SalesInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.SalesInvoiceDetailID = SalesInvoiceDetails.SalesInvoiceDetailID AND (AccountInvoiceDetails.EntryDate < SalesInvoiceDetails.EntryDate OR CAST(AccountInvoiceDetails.EntryDate AS DATE) <> CAST(SalesInvoiceDetails.EntryDate AS DATE)) ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("AccountInvoicePostSaveValidate", queryArray);
        }



        private void AccountInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("AccountInvoices", "AccountInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.AccountInvoice));
            this.totalBikePortalsEntities.CreateTrigger("AccountInvoiceInitReference", simpleInitReference.CreateQuery());
        }
    }
}
