namespace MVCClient.ViewModels.Helpers
{
    public interface ISupplierAutoCompleteViewModel
    {
        int SupplierID { get; set; }
        string CustomerName { get; set; }
        string CustomerAttentionName { get; set; }
        string CustomerAddressNo { get; set; }
        string CustomerEntireTerritoryEntireName { get; set; }
        string CustomerTelephone { get; set; }
    }
}
