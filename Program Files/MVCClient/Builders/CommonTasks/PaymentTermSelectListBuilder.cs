using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCClient.Builders.CommonTasks
{
    public class PaymentTermSelectListBuilder : IPaymentTermSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForPaymentTerms(IEnumerable<PaymentTerm> paymentTerms)
        {
            return paymentTerms.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.PaymentTermID.ToString() }).ToList();
        }
    }
}