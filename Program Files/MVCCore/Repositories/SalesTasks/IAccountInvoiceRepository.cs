using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCBase.Enums;
using System;


namespace MVCCore.Repositories.SalesTasks
{
    public interface IAccountInvoiceRepository : IGenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>
    {
        void ClearAccountInvoiceApi(int? accountInvoiceID);
        void UpdateAccountInvoiceApi(int? accountInvoiceID, string vATInvoiceSeries, int? apiSerialID, string apiSerialString, string apiMessage);

        List<AccountInvoiceSheet> GetAccountInvoiceSheet(int? accountInvoiceID);
    }

    public interface IAccountInvoiceAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PendingSalesInvoice> GetPendingSalesInvoices(int salesInvoiceID, string aspUserID, int locationID, int salesInvoiceTypeID, DateTime fromDate, DateTime toDate, int accountInvoiceID, string salesInvoiceDetailIDs);
    }
}
