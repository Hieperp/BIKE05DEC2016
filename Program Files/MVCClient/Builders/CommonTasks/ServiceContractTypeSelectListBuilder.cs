using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{
    public class ServiceContractTypeSelectListBuilder : IServiceContractTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForServiceContractTypes(IEnumerable<ServiceContractType> paymentTerms)
        {
            return paymentTerms.Select(pt => new SelectListItem { Text = pt.Description, Value = pt.ServiceContractTypeID.ToString() }).ToList();
        }
    }
}