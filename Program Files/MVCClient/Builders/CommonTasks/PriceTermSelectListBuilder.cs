using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{  
    public class PriceTermSelectListBuilder : IPriceTermSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForPriceTerms(IEnumerable<PriceTerm> priceTerms)
        {
            return priceTerms.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.PriceTermID.ToString() }).ToList();
        }
    }
}