using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;
using MVCData.Helpers.SqlProgrammability;

namespace MVCData.Helpers.SqlProgrammability.PurchaseTasks
{
    public class PurchaseOrder
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public PurchaseOrder(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPurchaseOrderIndexes();

            this.PurchaseOrderEditable();
            this.PurchaseOrderInitReference();
        }

        private void GetPurchaseOrderIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PurchaseOrders.PurchaseOrderID, CAST(PurchaseOrders.EntryDate AS DATE) AS EntryDate, PurchaseOrders.Reference, PurchaseOrders.ConfirmReference, PurchaseOrders.ConfirmDate, Locations.Code AS LocationCode, Suppliers.Name + ',    ' + Suppliers.AddressNo AS SupplierDescription, PurchaseOrders.TotalQuantity, PurchaseOrders.TotalGrossAmount, PurchaseOrders.Description " + "\r\n";
            queryString = queryString + "       FROM        PurchaseOrders INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON PurchaseOrders.EntryDate >= @FromDate AND PurchaseOrders.EntryDate <= @ToDate AND PurchaseOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.PurchaseOrder + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = PurchaseOrders.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Suppliers ON PurchaseOrders.SupplierID = Suppliers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetPurchaseOrderIndexes", queryString);
        }

        private void PurchaseOrderEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PurchaseOrderID FROM PurchaseInvoiceDetails WHERE PurchaseOrderID = @EntityID ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("PurchaseOrderEditable", queryArray);
        }
        
        private void PurchaseOrderInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("PurchaseOrders", "PurchaseOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.PurchaseOrder));
            this.totalBikePortalsEntities.CreateTrigger("PurchaseOrderInitReference", simpleInitReference.CreateQuery());
        }

    }
}
