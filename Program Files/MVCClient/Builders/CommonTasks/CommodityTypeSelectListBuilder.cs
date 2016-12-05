using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{  
    public class CommodityTypeSelectListBuilder : ICommodityTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityCategories(IEnumerable<CommodityType> commodityTypes)
        {
            return commodityTypes.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityTypeID.ToString() }).ToList();
        }
    }
}