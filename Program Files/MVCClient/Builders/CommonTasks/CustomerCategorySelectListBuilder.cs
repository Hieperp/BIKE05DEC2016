using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{  
    public class CustomerCategorySelectListBuilder : ICustomerCategorySelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCustomerCategories(IEnumerable<CustomerCategory> customerCategories)
        {
            return customerCategories.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CustomerCategoryID.ToString() }).ToList();
        }
    }
}