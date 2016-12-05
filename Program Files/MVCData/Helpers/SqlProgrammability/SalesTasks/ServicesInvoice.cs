using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class ServicesInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public ServicesInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetServicesInvoiceIndexes();

            this.GetRelatedPartsInvoiceValue();

            this.ServicesInvoicePostSaveValidate();

            this.ServicesInvoiceEditable();
            this.ServicesInvoiceDeletable();

            this.SearchServiceInvoices();
            this.SalesInvoicePrint();
        }


        private void GetServicesInvoiceIndexes()
        {
            this.totalBikePortalsEntities.CreateStoredProcedure("GetServicesInvoiceIndexes", this.BUILDSQLServicesInvoiceIndexes(true));
            this.totalBikePortalsEntities.CreateStoredProcedure("SearchServicesInvoiceIndexes", this.BUILDSQLServicesInvoiceIndexes(false));
        }

        private string BUILDSQLServicesInvoiceIndexes(bool getOrSearch)
        {   //getOrSearch = true: get; false: search
            string queryString;

            queryString = (getOrSearch ? " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " : " @ServiceContractID int") + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoices.SalesInvoiceID, CAST(SalesInvoices.EntryDate AS DATE) AS EntryDate, SalesInvoices.Reference, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, Commodities.Name AS CommodityName, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.LicensePlate, SalesInvoices.TotalGrossAmount, SalesInvoices.IsFinished " + "\r\n";
            queryString = queryString + "       FROM        SalesInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON SalesInvoices.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + (getOrSearch ? " AND SalesInvoices.EntryDate >= @FromDate AND SalesInvoices.EntryDate <= @ToDate AND SalesInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.ServicesInvoice + " AND AccessControls.AccessLevel > 0) " : " AND SalesInvoices.ServiceContractID = @ServiceContractID") + " AND Locations.LocationID = SalesInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON SalesInvoices.CustomerID = Customers.CustomerID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   ServiceContracts ON SalesInvoices.ServiceContractID = ServiceContracts.ServiceContractID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON ServiceContracts.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }

        /// <summary>
        /// Get QuantityAvailable (Remaining) Commodities BY EVERY GoodsReceiptDetailID (THIS MEANS: BY EVERY GoodsReceiptDetails LINE)
        /// </summary>
        private void GetRelatedPartsInvoiceValue()
        {
            string queryString = " @ServiceInvoiceID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      COUNT(SalesInvoiceID) AS NoInvoice, SUM(TotalQuantity) AS TotalQuantity, SUM(TotalGrossAmount) AS TotalGrossAmount, MIN(SalesInvoiceID) AS MinSalesInvoiceID, MIN(Reference) AS MinReference " + "\r\n";
            queryString = queryString + "       FROM        SalesInvoices " + "\r\n";
            queryString = queryString + "       WHERE       ServiceInvoiceID = @ServiceInvoiceID " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetRelatedPartsInvoiceValue", queryString);
        }


        private void ServicesInvoicePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Contract Date: ' + CAST(ServiceContracts.EntryDate AS nvarchar) FROM SalesInvoices INNER JOIN ServiceContracts ON SalesInvoices.SalesInvoiceID = @EntityID AND SalesInvoices.ServiceContractID = ServiceContracts.ServiceContractID AND SalesInvoices.EntryDate < ServiceContracts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Part Date: ' + CAST(SalesInvoices.EntryDate AS nvarchar) FROM SalesInvoices INNER JOIN SalesInvoices AS ServiceInvoices ON ServiceInvoices.SalesInvoiceID = @EntityID AND SalesInvoices.ServiceInvoiceID = ServiceInvoices.SalesInvoiceID AND SalesInvoices.EntryDate < ServiceInvoices.EntryDate ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("ServicesInvoicePostSaveValidate", queryArray);
        }






        private void ServicesInvoiceEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoices WHERE SalesInvoiceID = @EntityID AND IsFinished = 1 ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoiceDetails WHERE SalesInvoiceID = @EntityID AND NOT AccountInvoiceID IS NULL ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("ServicesInvoiceEditable", queryArray);
        }

        private void ServicesInvoiceDeletable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SalesInvoiceID FROM SalesInvoices WHERE ServiceInvoiceID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("ServicesInvoiceDeletable", queryArray);
        }







        private void SearchServiceInvoices()
        {
            string querySQL = " SELECT SalesInvoices.SalesInvoiceID, SalesInvoices.EntryDate, SalesInvoices.Reference, SalesInvoices.CustomerID, SalesInvoices.ServiceContractID, ServiceContracts.Reference AS ServiceContractReference, ServiceContracts.CommodityID AS ServiceContractCommodityID, ServiceContracts.LicensePlate, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.ColorCode, ServiceContracts.PurchaseDate, ServiceContracts.AgentName, SalesInvoices.QuotationID, SalesInvoices.ServiceLineID FROM SalesInvoices INNER JOIN ServiceContracts ON SalesInvoices.ServiceContractID = ServiceContracts.ServiceContractID WHERE SalesInvoices.LocationID = @LocationID AND SalesInvoices.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + " AND (SalesInvoices.SalesInvoiceID = @ServiceInvoiceID OR (@IsFinished = -1 OR SalesInvoices.IsFinished = @IsFinished)) ";

            string queryString = " @LocationID int, @ServiceInvoiceID int, @SearchText nvarchar(100), @IsFinished int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @ServiceInvoices TABLE (SalesInvoiceID int, EntryDate datetime NOT NULL, Reference nvarchar(10) NULL, CustomerID int NOT NULL, ServiceContractID int, ServiceContractReference nvarchar(10) NULL, ServiceContractCommodityID int NOT NULL, LicensePlate nvarchar(60) NULL, ChassisCode nvarchar(60) NULL, EngineCode nvarchar(60) NULL, ColorCode nvarchar(60) NULL, PurchaseDate datetime NULL, AgentName nvarchar(100) NULL, QuotationID int NULL, ServiceLineID int NOT NULL)" + "\r\n";


            queryString = queryString + "       IF (@SearchText = '') " + "\r\n";
            queryString = queryString + "           INSERT INTO @ServiceInvoices " + querySQL + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               INSERT INTO @ServiceInvoices " + querySQL + " AND ServiceContracts.LicensePlate LIKE '%' + @SearchText + '%' " + "\r\n";
            queryString = queryString + "               IF (@@ROWCOUNT <= 0) " + "\r\n";
            queryString = queryString + "                   INSERT INTO @ServiceInvoices " + querySQL + " AND (ServiceContracts.ChassisCode LIKE '%' + @SearchText + '%' OR ServiceContracts.EngineCode LIKE '%' + @SearchText + '%') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       SELECT  ServiceInvoices.SalesInvoiceID, ServiceInvoices.EntryDate, ServiceInvoices.Reference, ServiceInvoices.QuotationID, Quotations.Reference AS QuotationReference, Quotations.EntryDate AS QuotationEntryDate, " + "\r\n";
            queryString = queryString + "               ServiceInvoices.CustomerID, Customers.Name AS CustomerName, Customers.Birthday AS CustomerBirthday, Customers.Telephone + ' ' + Customers.Facsimile AS CustomerTelephone, Customers.AddressNo AS CustomerAddressNo, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, " + "\r\n";
            queryString = queryString + "               ServiceInvoices.ServiceContractID, ServiceInvoices.ServiceContractReference, ServiceInvoices.ServiceContractCommodityID, Commodities.Code AS ServiceContractCommodityCode, Commodities.Name AS ServiceContractCommodityName, ServiceInvoices.LicensePlate AS ServiceContractLicensePlate, ServiceInvoices.ColorCode AS ServiceContractColorCode, ServiceInvoices.ChassisCode AS ServiceContractChassisCode, ServiceInvoices.EngineCode AS ServiceContractEngineCode, ServiceInvoices.PurchaseDate AS ServiceContractPurchaseDate, ServiceInvoices.AgentName AS ServiceContractAgentName, ServiceInvoices.ServiceLineID " + "\r\n";
            queryString = queryString + "       FROM    @ServiceInvoices ServiceInvoices INNER JOIN " + "\r\n";
            queryString = queryString + "               Commodities ON ServiceInvoices.ServiceContractCommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "               Customers ON ServiceInvoices.CustomerID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "               EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID LEFT JOIN " + "\r\n";
            queryString = queryString + "               Quotations ON ServiceInvoices.QuotationID = Quotations.QuotationID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SearchServiceInvoices", queryString);
        }




        private void SalesInvoicePrint()
        {
            string queryString = " @SalesInvoiceID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalSalesInvoiceID int    SET @LocalSalesInvoiceID = @SalesInvoiceID" + "\r\n";

            queryString = queryString + "       SELECT          SalesInvoices.SalesInvoiceID, IIF(SalesInvoices.SalesInvoiceID = @LocalSalesInvoiceID, SalesInvoices.EntryDate, NULL) AS EntryDate, GetDate() AS PrintedDate, SalesInvoices.Reference, IIF(SalesInvoices.SalesInvoiceID = @LocalSalesInvoiceID, SalesInvoices.Reference, NULL) AS SVReference, SalesInvoices.VATInvoiceNo, SalesInvoices.VATInvoiceDate, SalesInvoices.VATInvoiceSeries, " + "\r\n";
            queryString = queryString + "                       SalesInvoices.LocationID, Locations.OfficialName AS LocationName, Locations.Address AS LocationAddress, Locations.Telephone AS LocationTelephone, Locations.Facsimile AS LocationFacsimile, AspNetUsers.FirstName + ' ' + AspNetUsers.LastName AS PreparedPersonName, Employees.Name AS EmployeeName, " + "\r\n";
            queryString = queryString + "                       IIF(SalesInvoices.SalesInvoiceID = @LocalSalesInvoiceID, SalesInvoices.SalesInvoiceTypeID, NULL) AS SVSalesInvoiceTypeID, SalesInvoices.SalesInvoiceTypeID, SalesInvoiceTypes.Name AS SalesInvoiceTypeName, Customers.Name AS CustomerName, Customers.Birthday, Customers.Telephone, Customers.Facsimile, Customers.AddressNo, EntireTerritories.EntireName AS EntireTerritoryEntireName, SalesInvoices.Damages, SalesInvoices.Causes, SalesInvoices.Solutions, " + "\r\n";
            queryString = queryString + "                       ServiceContracts.EntryDate AS ServiceContractEntryDate, Vehicles.Name AS VehicleName, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.LicensePlate, ServiceContracts.ColorCode, ServiceContracts.AgentName, ServiceContracts.EndingMeters, ServiceContracts.EndingDate, SalesInvoices.CurrentMeters, SalesInvoices.IsMajorRepair, SalesInvoices.NextMaintenance, " + "\r\n";
            queryString = queryString + "                       IIF(Commodities.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Consumables + ", " + (int)GlobalEnums.CommodityTypeID.Parts + ", Commodities.CommodityTypeID) AS CommodityTypeID, SalesInvoiceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.ListedPrice, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, " + "\r\n";
            queryString = queryString + "                       (SELECT dbo.SayVND(SUM(TotalGrossAmount)) FROM SalesInvoices WHERE SalesInvoiceID = @LocalSalesInvoiceID OR ServiceInvoiceID = @LocalSalesInvoiceID) AS TotalGrossAmountInWords " + "\r\n";
            queryString = queryString + "       FROM            SalesInvoices INNER JOIN " + "\r\n";
            queryString = queryString + "                       SalesInvoiceTypes ON SalesInvoices.SalesInvoiceTypeID = SalesInvoiceTypes.SalesInvoiceTypeID AND (SalesInvoices.SalesInvoiceID = @LocalSalesInvoiceID OR SalesInvoices.ServiceInvoiceID = @LocalSalesInvoiceID) INNER JOIN " + "\r\n";
            queryString = queryString + "                       Locations ON SalesInvoices.LocationID = Locations.LocationID INNER JOIN " + "\r\n";

            queryString = queryString + "                       Customers ON SalesInvoices.CustomerID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "                       EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID LEFT JOIN " + "\r\n";
            queryString = queryString + "                       Employees ON SalesInvoices.SalesInvoiceID = @LocalSalesInvoiceID AND SalesInvoices.EmployeeID = Employees.EmployeeID LEFT JOIN " + "\r\n";
            queryString = queryString + "                       AspNetUsers ON SalesInvoices.SalesInvoiceID = @LocalSalesInvoiceID AND SalesInvoices.PreparedPersonID = AspNetUsers.UserID LEFT JOIN " + "\r\n";
            queryString = queryString + "                       ServiceContracts ON SalesInvoices.ServiceContractID = ServiceContracts.ServiceContractID LEFT JOIN " + "\r\n";
            queryString = queryString + "                       Commodities AS Vehicles ON ServiceContracts.CommodityID = Vehicles.CommodityID LEFT JOIN " + "\r\n";
            queryString = queryString + "                       SalesInvoiceDetails ON SalesInvoices.SalesInvoiceID = SalesInvoiceDetails.SalesInvoiceID LEFT JOIN " + "\r\n";
            queryString = queryString + "                       Commodities ON SalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoicePrint", queryString);

        }


    }
}

