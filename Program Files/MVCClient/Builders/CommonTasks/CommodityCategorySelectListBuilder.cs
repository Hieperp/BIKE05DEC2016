using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{  
    public class CommodityCategorySelectListBuilder : ICommodityCategorySelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityCategories(IEnumerable<CommodityCategory> commodityCategories)
        {
            return commodityCategories.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityCategoryID.ToString() }).ToList();
        }
    }
}