using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{
    public class WarehouseSelectListBuilder : IWarehouseSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForWarehouses(IEnumerable<Warehouse> warehouses)
        {
            return warehouses.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.WarehouseID.ToString() }).ToList();
        }
    }
}