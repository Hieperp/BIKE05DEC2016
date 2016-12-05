using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{  
    public class CustomerTypeSelectListBuilder : ICustomerTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCustomerCategories(IEnumerable<CustomerType> customerTypes)
        {
            return customerTypes.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CustomerTypeID.ToString() }).ToList();
        }
    }
}