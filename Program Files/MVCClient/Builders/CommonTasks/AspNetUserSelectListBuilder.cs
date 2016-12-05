using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{
    public class AspNetUserSelectListBuilder : IAspNetUserSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForAspNetUsers(IEnumerable<AspNetUser> aspNetUsers, int userID)
        {
            return aspNetUsers.OrderBy(od => od.UserID == userID ? 1 : 2).Select(pt => new SelectListItem { Text = pt.LastName + ' ' + pt.FirstName, Value = pt.UserID.ToString() }).ToList();
        }
    }
}