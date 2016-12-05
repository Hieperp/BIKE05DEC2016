using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{

    public interface IServiceContractTypeSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForServiceContractTypes(IEnumerable<ServiceContractType> paymentTerms);
    }
}