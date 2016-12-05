using System;
namespace MVCClient.ViewModels.Helpers
{
    public interface ICustomerAutoCompleteViewModel
    {
        int CustomerID { get; set; }
        string CustomerName { get; set; }
        Nullable<System.DateTime> CustomerBirthday { get; set; }
        string CustomerVATCode { get; set; }
        string CustomerTelephone { get; set; }
        string CustomerAddressNo { get; set; }
        string CustomerEntireTerritoryEntireName { get; set; }
    }
}
