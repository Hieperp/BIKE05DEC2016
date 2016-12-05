using System.Web.Mvc;
using System.Collections.Generic;

using MVCModel.Models;
using MVCClient.ViewModels.CommonTasks;


namespace MVCClient.Builders.CommonTasks
{
    public interface ICustomerViewModelSelectListBuilder : IViewModelSelectListBuilder<CustomerViewModel>
    {
        IEnumerable<SelectListItem> BuildSelectListItemsCustomers(IEnumerable<Customer> customers);
        IEnumerable<SelectListItem> BuildSelectListItemsSuppliers(IEnumerable<Customer> suppliers);
    }
}