using System.Text;
using MVCModel.Models;
using MVCBase.Enums;
using System;


namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class SalesInvoice
    {

        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public SalesInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.SalesInvoiceJournal();

            this.ServiceInvoiceJournal();// ServiceInvoiceJournal va SalesInvoiceByServiceContract: GAN GIONG NHAU. TUY NHIEN, NGAY 12-JUN-2016: BO SUNG ServiceInvoiceJournal NHAM LAM REPORT BY EMPLOYEE
            this.SalesInvoiceByServiceContract();

            this.GoodsReceiptJournal();
        }


        #region SalesInvoiceJournal

        private string BUILDHeader(bool localParameter)
        {
            string queryString = " @LocationID int, @SalesInvoiceTypeID int, @FromDate DateTime, @ToDate DateTime, @WithAccountInvoice bit, @IsGroupByPrice bit, @IncludePromotionID int, @IsHideDetail bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SET NOCOUNT ON; " + "\r\n";

            if (localParameter)
            {
                queryString = queryString + "       DECLARE     @LocalLocationID int, @LocalSalesInvoiceTypeID int, @LocalFromDate DateTime, @LocalToDate DateTime, @LocalWithAccountInvoice bit, @LocalIsGroupByPrice bit, @LocalIncludePromotionID int, @LocalIsHideDetail bit " + "\r\n";
                queryString = queryString + "       SET         @LocalLocationID = @LocationID  SET @LocalSalesInvoiceTypeID = @SalesInvoiceTypeID  SET @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate  SET @LocalWithAccountInvoice = @WithAccountInvoice   SET @LocalIsGroupByPrice = @IsGroupByPrice    SET @LocalIncludePromotionID = @IncludePromotionID       SET @LocalIsHideDetail = @IsHideDetail " + "\r\n";
            }

            return queryString;
        }

        private string BUILDParameter(bool localParameter)
        {
            return " @" + (localParameter ? "Local" : "") + "LocationID, @" + (localParameter ? "Local" : "") + "SalesInvoiceTypeID, @" + (localParameter ? "Local" : "") + "FromDate, @" + (localParameter ? "Local" : "") + "ToDate, @" + (localParameter ? "Local" : "") + "WithAccountInvoice, @" + (localParameter ? "Local" : "") + "IsGroupByPrice, @" + (localParameter ? "Local" : "") + "IncludePromotionID, @" + (localParameter ? "Local" : "") + "IsHideDetail ";
        }

        /// <summary>
        /// AccountInvoiceJournal
        /// SalesInvoiceDetails
        /// SalesInvoiceJournal
        /// SalesInvoiceMargin
        /// </summary>
        private void SalesInvoiceJournal()
        {
            this.SalesInvoiceJournal02();

            string queryString = this.BUILDHeader(true);
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.AllInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.AllInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice + ")  " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.PartsInvoice) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n"; //GlobalEnums.SalesInvoiceTypeID.ServicesInvoice
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.ServicesInvoice) + "\r\n";
            queryString = queryString + "    END " + "\r\n";
            
            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoiceJournal", queryString);
        }


        private string SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalLocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildMaster(salesInvoiceTypeID, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildMaster(salesInvoiceTypeID, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string SalesInvoiceJournalBuildMaster(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalWithAccountInvoice = 0) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "                   IF          (@LocalIncludePromotionID = 0) " + "\r\n";
            queryString = queryString + "                           EXEC SalesInvoiceJournal02" + salesInvoiceTypeID.ToString().Substring(0, 1) + locationFilter.ToString().Substring(0, 1) + false.ToString().Substring(0, 1) + (0+1).ToString().Substring(0, 1) + this.BUILDParameter(true) + "\r\n";
            //queryString = queryString + "                           " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, false, 0) + "\r\n";
            queryString = queryString + "                   ELSE    IF  (@LocalIncludePromotionID = -1) " + "\r\n";
            queryString = queryString + "                           EXEC SalesInvoiceJournal02" + salesInvoiceTypeID.ToString().Substring(0, 1) + locationFilter.ToString().Substring(0, 1) + false.ToString().Substring(0, 1) + (-1 + 1).ToString().Substring(0, 1) + this.BUILDParameter(true) + "\r\n";
            //queryString = queryString + "                           " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, false, -1) + "\r\n";
            queryString = queryString + "                   ELSE    " + "\r\n"; //(@LocalIncludePromotionID = 1)
            queryString = queryString + "                           EXEC SalesInvoiceJournal02" + salesInvoiceTypeID.ToString().Substring(0, 1) + locationFilter.ToString().Substring(0, 1) + false.ToString().Substring(0, 1) + (1 + 1).ToString().Substring(0, 1) + this.BUILDParameter(true) + "\r\n";
            //queryString = queryString + "                           " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, false, 1) + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE        " + "\r\n";

            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "                   IF          (@LocalIncludePromotionID = 0) " + "\r\n";
            queryString = queryString + "                           EXEC SalesInvoiceJournal02" + salesInvoiceTypeID.ToString().Substring(0, 1) + locationFilter.ToString().Substring(0, 1) + true.ToString().Substring(0, 1) + (0 + 1).ToString().Substring(0, 1) + this.BUILDParameter(true) + "\r\n";
            //queryString = queryString + "                           " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, true, 0) + "\r\n";
            queryString = queryString + "                   ELSE    IF  (@LocalIncludePromotionID = -1) " + "\r\n";
            queryString = queryString + "                           EXEC SalesInvoiceJournal02" + salesInvoiceTypeID.ToString().Substring(0, 1) + locationFilter.ToString().Substring(0, 1) + true.ToString().Substring(0, 1) + (-1 + 1).ToString().Substring(0, 1) + this.BUILDParameter(true) + "\r\n";
            //queryString = queryString + "                           " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, true, -1) + "\r\n";
            queryString = queryString + "                   ELSE    " + "\r\n"; //(@LocalIncludePromotionID = 1)
            queryString = queryString + "                           EXEC SalesInvoiceJournal02" + salesInvoiceTypeID.ToString().Substring(0, 1) + locationFilter.ToString().Substring(0, 1) + true.ToString().Substring(0, 1) + (1 + 1).ToString().Substring(0, 1) + this.BUILDParameter(true) + "\r\n";
            //queryString = queryString + "                           " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, true, 1) + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }


        private void SalesInvoiceJournal02()
        {
            bool[] boolArray = new bool[2] { true, false };

            foreach (GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID in Enum.GetValues(typeof(GlobalEnums.SalesInvoiceTypeID)))
            {
                foreach (bool locationFilter in boolArray)
                {
                    foreach (bool withAccountInvoice in boolArray)
                    {
                        for (int includePromotionID = 0; includePromotionID <= 2; includePromotionID++)
                        {
                            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoiceJournal02" + salesInvoiceTypeID.ToString().Substring(0, 1) + locationFilter.ToString().Substring(0, 1) + withAccountInvoice.ToString().Substring(0, 1) + includePromotionID.ToString().Substring(0, 1), this.BUILDHeader(true) + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, withAccountInvoice, includePromotionID-1));
                        }
                    }
                }
            }
        }
        //----
        private string SalesInvoiceJournalBuildDetail(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter, bool withAccountInvoice, int includePromotionID)
        {
            string queryString = "";

            if (!withAccountInvoice && includePromotionID != 1)
            {
                queryString = queryString + "   BEGIN " + "\r\n";
                queryString = queryString + "       IF          (@LocalIsHideDetail = 0) " + "\r\n";
                queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetailHideDetail(salesInvoiceTypeID, locationFilter, withAccountInvoice, includePromotionID, false) + "\r\n";
                queryString = queryString + "       ELSE        " + "\r\n";
                queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetailHideDetail(salesInvoiceTypeID, locationFilter, withAccountInvoice, includePromotionID, true) + "\r\n";
                queryString = queryString + "   END " + "\r\n";
            }
            else
                queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetailHideDetail(salesInvoiceTypeID, locationFilter, withAccountInvoice, includePromotionID, false) + "\r\n";

            return queryString;
        }

        private string SalesInvoiceJournalBuildDetailHideDetail(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter, bool withAccountInvoice, int includePromotionID, bool isHideDetail)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalIsGroupByPrice = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetailHideDetailGroupByPrice(salesInvoiceTypeID, locationFilter, withAccountInvoice, includePromotionID, isHideDetail, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetailHideDetailGroupByPrice(salesInvoiceTypeID, locationFilter, withAccountInvoice, includePromotionID, isHideDetail, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string SalesInvoiceJournalBuildDetailHideDetailGroupByPrice(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter, bool withAccountInvoice, int includePromotionID, bool isHideDetail, bool isGroupByPrice)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.SalesInvoiceID, SalesInvoiceDetails.SalesInvoiceDetailID, SalesInvoiceDetails.SalesInvoiceTypeID, SalesInvoiceDetails.EntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code, Commodities.Name, SalesInvoiceDetails.CommodityTypeID, SalesInvoiceDetails.WarehouseID, IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + ", SalesInvoiceDetails.SalesInvoiceID, SalesInvoiceDetails.ServiceInvoiceID) AS ServiceInvoiceID, " + "\r\n";
            queryString = queryString + "                   Locations.Code AS LocationCode, VWCommodityCategories.CommodityCategoryID, VWCommodityCategories.Name1 AS CommodityCategory1, VWCommodityCategories.Name2 AS CommodityCategory2, VWCommodityCategories.Name3 AS CommodityCategory3, " + "\r\n";

            if (!isHideDetail)
                queryString = queryString + "               Customers.CustomerID, Customers.Name AS CustomerName, Customers.Birthday, Customers.IsFemale, Customers.Telephone, Customers.Facsimile, Customers.AddressNo, EntireTerritories.Name2 AS DistrictName, EntireTerritories.Name1 AS ProvinceName, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            else
                queryString = queryString + "               -1 AS CustomerID, '' AS CustomerName, NULL AS Birthday, 0 AS IsFemale, '' AS Telephone, NULL AS Facsimile, '' AS AddressNo, '' AS DistrictName, '' AS ProvinceName, '' AS CustomerCategoryName, " + "\r\n";

            if (withAccountInvoice)
                queryString = queryString + "               AccountInvoices.AccountInvoiceID, VATCustomers.Name AS VATCustomerName, VATCustomers.VATCode, AccountInvoices.VATInvoiceNo, AccountInvoices.VATInvoiceDate, AccountInvoices.VATInvoiceSeries, AccountInvoices.Description AS VATDescription, " + "\r\n";
            else
                queryString = queryString + "               NULL AS AccountInvoiceID, NULL AS VATCustomerName, NULL AS VATCode, NULL AS VATInvoiceNo, NULL AS VATInvoiceDate, NULL AS VATInvoiceSeries, NULL AS VATDescription, " + "\r\n";



            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice || salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice)
                queryString = queryString + "               GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            else
                queryString = queryString + "               NULL AS ChassisCode, NULL AS EngineCode, NULL AS ColorCode, " + "\r\n";


            queryString = queryString + "                   SalesInvoiceDetails.PromotionID, " + (includePromotionID == 1 ? " Promotions.Code AS PromotionCode, Promotions.Name AS PromotionName, " : " NULL AS PromotionCode, NULL AS PromotionName, ") + "\r\n";

            queryString = queryString + "                   SalesInvoiceDetails.Quantity, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, " + "\r\n";



            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice)
            {
                string costPrice = "IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice + ", GoodsReceiptDetails.UnitPrice, (IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice + ", ISNULL(WarehouseBalancePrice.UnitPrice, 0), 0)))";
                queryString = queryString + "               " + costPrice + " AS CostPrice, SalesInvoiceDetails.Quantity * (" + costPrice + ") AS CostAmount, " + "\r\n";
            }
            else
                if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice)
                    queryString = queryString + "           GoodsReceiptDetails.UnitPrice AS CostPrice, SalesInvoiceDetails.Quantity * GoodsReceiptDetails.UnitPrice AS CostAmount, " + "\r\n";
                else
                    if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.PartsInvoice)
                        queryString = queryString + "       ISNULL(WarehouseBalancePrice.UnitPrice, 0) AS CostPrice, SalesInvoiceDetails.Quantity * ISNULL(WarehouseBalancePrice.UnitPrice, 0) AS CostAmount, " + "\r\n";
                    else //salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.ServicesInvoice
                        queryString = queryString + "       0 AS CostPrice, 0 AS CostAmount, " + "\r\n";


            queryString = queryString + "                   IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + " OR NOT SalesInvoiceDetails.ServiceInvoiceID IS NULL, SalesInvoiceDetails.GrossAmount, 0) AS ServiceGrossAmount, IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + " OR NOT SalesInvoiceDetails.ServiceInvoiceID IS NULL, 0, SalesInvoiceDetails.GrossAmount) AS NonServiceGrossAmount, @LocalIsGroupByPrice * SalesInvoiceDetails.UnitPrice AS UnitPriceGroup, " + "\r\n"; //WHEN SalesInvoiceTypeID = GlobalEnums.SalesInvoiceTypeID.ServicesInvoice => SalesInvoiceDetails.ServiceInvoiceID IS ALWAYS NULL
            queryString = queryString + "                   " + (includePromotionID <= 0 ? "VWCommodityCategories.Name1" : "Promotions.Code") + " AS Group1Name, " + (includePromotionID <= 0 ? "VWCommodityCategories.Name2" : "VWCommodityCategories.Name1") + " AS Group2Name " + "\r\n";

            queryString = queryString + "       FROM        " + "\r\n";

            string querySalesInvoiceDetail = (includePromotionID == -1 ? "(SalesInvoiceDetails.IsBonus IS NULL OR SalesInvoiceDetails.IsBonus = 0) AND " : "") + " SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate " + (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice ? "" : " AND SalesInvoiceDetails.SalesInvoiceTypeID = @LocalSalesInvoiceTypeID") + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocalLocationID" : "") + "";
            if (!isHideDetail)
            {
                queryString = queryString + "                   SalesInvoiceDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON " + querySalesInvoiceDetail + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            }
            else //WHEN isHideDetail (THE CONDITION IS: if (!withAccountInvoice && includePromotionID != 1))
            {
                queryString = queryString + "                  (SELECT -1 AS SalesInvoiceID, -1 AS SalesInvoiceDetailID, -1 AS WarehouseID, -1 AS CustomerID, -1 AS PromotionID, -1 AS AccountInvoiceID, 0 AS IsBonus, IIF(ServiceInvoiceID IS NULL, NULL, -1) AS ServiceInvoiceID, DATEFROMPARTS(YEAR(EntryDate), MONTH(EntryDate), 1) AS EntryDate, LocationID, CommodityTypeID, CommodityID, SalesInvoiceTypeID, GoodsReceiptDetailID, SUM(Quantity) AS Quantity, 0 AS DiscountPercent, " + (isGroupByPrice ? "UnitPrice" : "0 AS UnitPrice") + ", 0 AS VATPercent, 0 AS GrossPrice, SUM(Amount) AS Amount, SUM(VATAmount) AS VATAmount, SUM(GrossAmount) AS GrossAmount " + "\r\n";
                queryString = queryString + "                   FROM SalesInvoiceDetails WHERE " + querySalesInvoiceDetail + (includePromotionID == -1 ? " AND SalesInvoiceDetails.PromotionID IS NULL" : "") + " GROUP BY LocationID, CommodityTypeID, CommodityID, SalesInvoiceTypeID, GoodsReceiptDetailID, IIF(ServiceInvoiceID IS NULL, NULL, -1), DATEFROMPARTS(YEAR(EntryDate), MONTH(EntryDate), 1)" + (isGroupByPrice ? ", UnitPrice" : "") + ") AS SalesInvoiceDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON SalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            }

            if (withAccountInvoice)
            {
                queryString = queryString + "               INNER JOIN AccountInvoices ON SalesInvoiceDetails.AccountInvoiceID = AccountInvoices.AccountInvoiceID " + "\r\n";
                queryString = queryString + "               INNER JOIN Customers VATCustomers ON AccountInvoices.CustomerID = VATCustomers.CustomerID " + "\r\n";
            }

            if (includePromotionID == 1)
                queryString = queryString + "               INNER JOIN Promotions ON SalesInvoiceDetails.IsBonus = 1 AND SalesInvoiceDetails.PromotionID = Promotions.PromotionID " + "\r\n";

            if (!isHideDetail)
            {
                queryString = queryString + "               INNER JOIN Customers ON SalesInvoiceDetails.CustomerID = Customers.CustomerID " + "\r\n";
                queryString = queryString + "               INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
                queryString = queryString + "               INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            }
            queryString = queryString + "                   INNER JOIN Locations ON SalesInvoiceDetails.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID " + "\r\n";

            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice)
            {
                queryString = queryString + "               LEFT JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
                queryString = queryString + "               LEFT JOIN WarehouseBalancePrice ON SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID AND MONTH(SalesInvoiceDetails.EntryDate) = MONTH(WarehouseBalancePrice.EntryDate) AND YEAR(SalesInvoiceDetails.EntryDate) = YEAR(WarehouseBalancePrice.EntryDate) " + "\r\n";
            }
            else
                if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice)
                    queryString = queryString + "           INNER JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
                else
                    if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.PartsInvoice)
                        queryString = queryString + "       LEFT JOIN WarehouseBalancePrice ON SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID AND MONTH(SalesInvoiceDetails.EntryDate) = MONTH(WarehouseBalancePrice.EntryDate) AND YEAR(SalesInvoiceDetails.EntryDate) = YEAR(WarehouseBalancePrice.EntryDate) " + "\r\n";


            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        #endregion SalesInvoiceJournal



        private void ServiceInvoiceJournal()
        {
            string queryString = " @LocationID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";

            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalLocationID int, @LocalFromDate DateTime, @LocalToDate DateTime " + "\r\n";
            queryString = queryString + "       SET         @LocalLocationID = @LocationID  SET @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate  " + "\r\n";

            queryString = queryString + "       IF          (@LocalLocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.ServiceInvoiceJournalBUILSQL(false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "                   " + this.ServiceInvoiceJournalBUILSQL(true) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("ServiceInvoiceJournal", queryString);
        }


        private string ServiceInvoiceJournalBUILSQL(bool locationFilter)
        {
            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      UNIONSalesInvoiceDetails.EntryDate, UNIONSalesInvoiceDetails.LocationID, Locations.Code AS LocationCode, Locations.OfficialName AS LocationOfficialName, ServiceContracts.CommodityID AS VehicleID, ServiceVehicles.Code AS VehicleCode, ServiceVehicles.Name AS VehicleName, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.ColorCode, ServiceContracts.LicensePlate, SalesInvoices.RespondedDate, Employees.Name AS EmployeeName,  " + "\r\n";
            queryString = queryString + "                   CommodityCategories.CommodityCategoryID, CommodityCategories.Name AS CommodityCategoryName, Commodities.CommodityTypeID, UNIONSalesInvoiceDetails.CommodityID, Commodities.Code, Commodities.Name, UNIONSalesInvoiceDetails.Quantity, UNIONSalesInvoiceDetails.UnitPrice, UNIONSalesInvoiceDetails.VATPercent, UNIONSalesInvoiceDetails.GrossPrice, UNIONSalesInvoiceDetails.Amount, UNIONSalesInvoiceDetails.VATAmount, UNIONSalesInvoiceDetails.GrossAmount " + "\r\n";

            queryString = queryString + "       FROM       (SELECT      SalesInvoiceDetails.SalesInvoiceID, SalesInvoiceDetails.EntryDate, SalesInvoiceDetails.LocationID, SalesInvoiceDetails.EmployeeID, SalesInvoiceDetails.CommodityID, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount " + "\r\n";
            queryString = queryString + "                   FROM        SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "                   WHERE       SalesInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Services + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocalLocationID" : "") + " AND SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate " + "\r\n";

            queryString = queryString + "                   UNION ALL  " + "\r\n";

            queryString = queryString + "                   SELECT      ServiceInvoices.SalesInvoiceID, ServiceInvoices.EntryDate, ServiceInvoices.LocationID, ServiceInvoices.EmployeeID, SalesInvoiceDetails.CommodityID, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount " + "\r\n";
            queryString = queryString + "                   FROM        SalesInvoiceDetails  " + "\r\n";
            queryString = queryString + "                               INNER JOIN SalesInvoices ServiceInvoices ON (SalesInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Parts + " OR SalesInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Consumables + ") " + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocalLocationID" : "") + " AND SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate AND SalesInvoiceDetails.ServiceInvoiceID = ServiceInvoices.SalesInvoiceID " + "\r\n";

            queryString = queryString + "                  )UNIONSalesInvoiceDetails INNER JOIN Locations ON UNIONSalesInvoiceDetails.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON UNIONSalesInvoiceDetails.EmployeeID = Employees.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON UNIONSalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN SalesInvoices ON UNIONSalesInvoiceDetails.SalesInvoiceID = SalesInvoices.SalesInvoiceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ServiceContracts ON SalesInvoices.ServiceContractID = ServiceContracts.ServiceContractID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities AS ServiceVehicles ON ServiceContracts.CommodityID = ServiceVehicles.CommodityID" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }





        #region SalesInvoiceByServiceContract

        private void SalesInvoiceByServiceContract()
        {
            string queryString = " @LocationID int, @SalesInvoiceTypeID int, @FromDate DateTime, @ToDate DateTime, @IsRegularCheckUps bit, @IncludeVehiclesInvoice bit " + "\r\n";

            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalLocationID int, @LocalSalesInvoiceTypeID int, @LocalFromDate DateTime, @LocalToDate DateTime, @LocalIsRegularCheckUps bit, @LocalIncludeVehiclesInvoice bit " + "\r\n";
            queryString = queryString + "       SET         @LocalLocationID = @LocationID  SET @LocalSalesInvoiceTypeID = @SalesInvoiceTypeID  SET @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate  SET @LocalIsRegularCheckUps = @IsRegularCheckUps   SET @LocalIncludeVehiclesInvoice = @IncludeVehiclesInvoice " + "\r\n";

            queryString = queryString + "       DECLARE     @SalesInvoiceFilter TABLE (SalesInvoiceID int NOT NULL) " + "\r\n";

            queryString = queryString + "       IF          (@LocalIncludeVehiclesInvoice = 1) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuild(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuild(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoiceByServiceContract", queryString);
        }
        private string SalesInvoiceByServiceContractBuild(bool includeVehiclesInvoice)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalIsRegularCheckUps = 1) " + "\r\n";
            queryString = queryString + "               " + this.SalesInvoiceByServiceContractBuildMaster(GlobalEnums.SalesInvoiceTypeID.AllInvoice, true, includeVehiclesInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.AllInvoice + ") " + "\r\n";
            queryString = queryString + "               " + this.SalesInvoiceByServiceContractBuildMaster(GlobalEnums.SalesInvoiceTypeID.AllInvoice, false, includeVehiclesInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + ") " + "\r\n";
            queryString = queryString + "               " + this.SalesInvoiceByServiceContractBuildMaster(GlobalEnums.SalesInvoiceTypeID.ServicesInvoice, false, includeVehiclesInvoice) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n"; //LAY BAT KY GlobalEnums.SalesInvoiceTypeID LAM DAI DIEN, NGOAI TRU GlobalEnums.SalesInvoiceTypeID.ServicesInvoice, KHONG CO Y NGHIA GI (LAY GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice LAM DAI DIEN)
            queryString = queryString + "               " + this.SalesInvoiceByServiceContractBuildMaster(GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice, false, includeVehiclesInvoice) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string SalesInvoiceByServiceContractBuildMaster(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool isRegularCheckUps, bool includeVehiclesInvoice)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalLocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuildDetail(salesInvoiceTypeID, false, isRegularCheckUps, includeVehiclesInvoice) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuildDetail(salesInvoiceTypeID, true, isRegularCheckUps, includeVehiclesInvoice) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string SalesInvoiceByServiceContractBuildDetail(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter, bool isRegularCheckUps, bool includeVehiclesInvoice)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.ServicesInvoice)
            {
                queryString = queryString + "   INSERT INTO @SalesInvoiceFilter SELECT SalesInvoiceID FROM SalesInvoices WHERE " + this.SalesInvoiceByServiceContractBuildWHERE(salesInvoiceTypeID, locationFilter, "SalesInvoices") + "\r\n";
                queryString = queryString + "   INSERT INTO @SalesInvoiceFilter SELECT SalesInvoiceID FROM SalesInvoices WHERE ServiceInvoiceID IN (SELECT SalesInvoiceID FROM @SalesInvoiceFilter) " + "\r\n";
            }


            queryString = queryString + "       SELECT      SalesInvoiceDetails.SalesInvoiceID, SalesInvoiceDetails.SalesInvoiceDetailID, CASE WHEN SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + " THEN SalesInvoiceDetails.SalesInvoiceID ELSE SalesInvoiceDetails.ServiceInvoiceID END AS ServiceInvoiceID, SalesInvoiceDetails.EntryDate, CAST(SalesInvoiceDetails.EntryDate AS Date) AS EntryDateONLY, CAST(" + (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.ServicesInvoice ? "ISNULL(ServiceInvoices.EntryDate, SalesInvoiceDetails.EntryDate)" : "SalesInvoiceDetails.EntryDate") + " AS Date) AS ServiceEntryDate, SalesInvoiceDetails.LocationID, Locations.Code AS LocationCode, SalesInvoiceDetails.SalesInvoiceTypeID, Employees.EmployeeID, Employees.Name AS EmployeeName, " + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails.CommodityID, Commodities.Code, Commodities.Name, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, " + "\r\n";
            queryString = queryString + "                   ServiceContracts.CommodityID AS VehicleID, ServiceVehicles.Code AS VehicleCode, ServiceVehicles.Name AS VehicleName, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.ColorCode, ServiceContracts.LicensePlate, SalesInvoiceDetails.CurrentMeters, " + "\r\n";
            queryString = queryString + "                   " + (includeVehiclesInvoice ? "ISNULL(VehicleInvoiceDetails.EntryDate, ServiceContracts.PurchaseDate)" : "ServiceContracts.PurchaseDate") + " AS PurchaseDate, " + (includeVehiclesInvoice ? "ISNULL(VehicleLocations.OfficialName, ServiceContracts.AgentName)" : "ServiceContracts.AgentName") + " AS AgentName, " + (includeVehiclesInvoice ? "VehicleLocations.OfficialName" : "''") + " AS VehicleLocationName, " + "\r\n";
            queryString = queryString + "                   Customers.CustomerID, Customers.Name AS CustomerName, Customers.Birthday, Customers.IsFemale, Customers.VATCode, Customers.Telephone, Customers.Facsimile, Customers.AddressNo, EntireTerritories.Name2 AS DistrictName, EntireTerritories.Name1 AS ProvinceName, CustomerCategories.Name AS CustomerCategoryName " + "\r\n";


            queryString = queryString + "       FROM        SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON " + (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.ServicesInvoice ? "SalesInvoiceDetails.SalesInvoiceID IN (SELECT SalesInvoiceID FROM @SalesInvoiceFilter)" : this.SalesInvoiceByServiceContractBuildWHERE(salesInvoiceTypeID, locationFilter, "SalesInvoiceDetails")) + (isRegularCheckUps ? " AND Commodities.IsRegularCheckUps = 1" : "") + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";


            queryString = queryString + "                   INNER JOIN ServiceContracts ON SalesInvoiceDetails.ServiceContractID = ServiceContracts.ServiceContractID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities AS ServiceVehicles ON ServiceContracts.CommodityID = ServiceVehicles.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON ServiceContracts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SalesInvoiceDetails.LocationID = Locations.LocationID " + "\r\n";

            if (includeVehiclesInvoice)
            {
                queryString = queryString + "               LEFT JOIN SalesInvoiceDetails VehicleInvoiceDetails ON ServiceContracts.SalesInvoiceDetailID = VehicleInvoiceDetails.SalesInvoiceDetailID " + "\r\n";
                queryString = queryString + "               LEFT JOIN Locations VehicleLocations ON VehicleInvoiceDetails.LocationID = VehicleLocations.LocationID " + "\r\n";
            }

            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.ServicesInvoice)
            {
                queryString = queryString + "               LEFT JOIN SalesInvoices AS ServiceInvoices ON SalesInvoiceDetails.ServiceInvoiceID = ServiceInvoices.SalesInvoiceID " + "\r\n";

            }
            queryString = queryString + "                   LEFT JOIN Employees ON SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + " AND  SalesInvoiceDetails.EmployeeID = Employees.EmployeeID " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string SalesInvoiceByServiceContractBuildWHERE(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter, string tableName)
        {
            return tableName + ".EntryDate >= @LocalFromDate AND " + tableName + ".EntryDate <= @LocalToDate " + (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice ? "" : " AND " + tableName + ".SalesInvoiceTypeID = @LocalSalesInvoiceTypeID") + (locationFilter ? " AND " + tableName + ".LocationID = @LocalLocationID" : "");
        }

        #endregion SalesInvoiceByServiceContract





















        #region GoodsReceiptJournal

        private void GoodsReceiptJournal()
        {
            string queryString = " @LocationID int, @GoodsReceiptTypeID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";

            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalLocationID int, @LocalGoodsReceiptTypeID int, @LocalFromDate DateTime, @LocalToDate DateTime " + "\r\n";
            queryString = queryString + "       SET         @LocalLocationID = @LocationID  SET @LocalGoodsReceiptTypeID = @GoodsReceiptTypeID  SET @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate " + "\r\n";

            queryString = queryString + "       IF  (@LocalGoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt + ") " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalGoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n"; //GlobalEnums.GoodsReceiptTypeID.StockTransfer
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID.StockTransfer) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GoodsReceiptJournal", queryString);
        }

        private string GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalLocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuildDetail(goodsReceiptTypeID, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuildDetail(goodsReceiptTypeID, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string GoodsReceiptJournalBuildDetail(GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID, bool locationFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt || goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice)
            {
                queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.GoodsReceiptTypeID, GoodsReceiptDetails.VoucherID, GoodsReceiptDetails.LocationID, Locations.Code AS LocationCode, GoodsReceiptDetails.CommodityID, Commodities.Code, Commodities.Name, Commodities.CommodityTypeID, CommodityTypes.Name AS CommodityTypeName, Commodities.CommodityCategoryID, " + "\r\n";
                queryString = queryString + "                   GoodsReceiptDetails.Quantity, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetails.VATPercent, GoodsReceiptDetails.GrossPrice, GoodsReceiptDetails.Amount, GoodsReceiptDetails.VATAmount, GoodsReceiptDetails.GrossAmount, " + "\r\n";
                queryString = queryString + "                   PurchaseInvoices.SupplierID AS VoucherOwnerID, Suppliers.OfficialName AS VoucherOwner, PurchaseInvoices.VATInvoiceNo AS VoucherReference, PurchaseInvoices.VATInvoiceDate AS VoucherDate, PurchaseOrders.ConfirmReference AS RootVoucherReference, PurchaseOrders.ConfirmDate AS RootVoucherDate, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode " + "\r\n";

                queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + (locationFilter ? " AND GoodsReceiptDetails.LocationID = @LocalLocationID" : "") + " AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

                queryString = queryString + "                   INNER JOIN PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID " + "\r\n";
                queryString = queryString + "                   INNER JOIN CommodityTypes ON Commodities.CommodityTypeID = CommodityTypes.CommodityTypeID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Locations ON GoodsReceiptDetails.LocationID = Locations.LocationID " + "\r\n";
                queryString = queryString + "                   INNER JOIN PurchaseOrders ON PurchaseInvoices.PurchaseOrderID = PurchaseOrders.PurchaseOrderID  " + "\r\n";
            }

            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt)
                queryString = queryString + "       UNION ALL   " + "\r\n";

            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt || goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer)
            {
                queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.GoodsReceiptTypeID, GoodsReceiptDetails.VoucherID, GoodsReceiptDetails.LocationID, Locations.Code AS LocationCode, GoodsReceiptDetails.CommodityID, Commodities.Code, Commodities.Name, Commodities.CommodityTypeID, CommodityTypes.Name AS CommodityTypeName, Commodities.CommodityCategoryID, " + "\r\n";
                queryString = queryString + "                   GoodsReceiptDetails.Quantity, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetails.VATPercent, GoodsReceiptDetails.GrossPrice, GoodsReceiptDetails.Amount, GoodsReceiptDetails.VATAmount, GoodsReceiptDetails.GrossAmount, " + "\r\n";
                queryString = queryString + "                   StockTransfers.LocationID AS VoucherOwnerID, StockTransferLocations.OfficialName AS VoucherOwner, StockTransfers.Reference AS VoucherReference, StockTransfers.EntryDate AS VoucherDate, TransferOrders.Reference AS RootVoucherReference, TransferOrders.EntryDate AS RootVoucherDate, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode " + "\r\n";

                queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + (locationFilter ? " AND GoodsReceiptDetails.LocationID = @LocalLocationID" : "") + " AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

                queryString = queryString + "                   INNER JOIN StockTransfers ON GoodsReceiptDetails.VoucherID = StockTransfers.StockTransferID " + "\r\n";
                queryString = queryString + "                   INNER JOIN CommodityTypes ON Commodities.CommodityTypeID = CommodityTypes.CommodityTypeID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Locations StockTransferLocations ON StockTransfers.LocationID = StockTransferLocations.LocationID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Locations ON GoodsReceiptDetails.LocationID = Locations.LocationID " + "\r\n";
                queryString = queryString + "                   LEFT JOIN TransferOrders ON StockTransfers.TransferOrderID = TransferOrders.TransferOrderID " + "\r\n";
            }

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }
        #endregion GoodsReceiptJournal

    }
}
