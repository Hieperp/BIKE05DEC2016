using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Repositories.SalesTasks;
using System;



namespace MVCData.Repositories.SalesTasks
{
    public class AccountInvoiceRepository : GenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>, IAccountInvoiceRepository
    {
        public AccountInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities) { }

        public void ClearAccountInvoiceApi(int? accountInvoiceID)
        {
            base.TotalBikePortalsEntities.ClearAccountInvoiceApi(accountInvoiceID);
        }

        public void UpdateAccountInvoiceApi(int? accountInvoiceID, int? apiSerialID, string apiSerialString, string responedMessage)
        {
            if (apiSerialID == 0) apiSerialID = null;
            base.TotalBikePortalsEntities.UpdateAccountInvoiceApi(accountInvoiceID, apiSerialID, apiSerialString, responedMessage);
        }

        public List<AccountInvoiceSheet> GetAccountInvoiceSheet(int? accountInvoiceID)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            List<AccountInvoiceSheet> accountInvoiceSheets = base.TotalBikePortalsEntities.AccountInvoiceSheet(accountInvoiceID).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return accountInvoiceSheets;
        }
    }


    public class AccountInvoiceAPIRepository : GenericAPIRepository, IAccountInvoiceAPIRepository
    {
        public AccountInvoiceAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetAccountInvoiceIndexes")
        {
        }
        public IEnumerable<PendingSalesInvoice> GetPendingSalesInvoices(int salesInvoiceID, string aspUserID, int locationID, int salesInvoiceTypeID, DateTime fromDate, DateTime toDate, int accountInvoiceID, string salesInvoiceDetailIDs)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingSalesInvoice> pendingSalesInvoices = base.TotalBikePortalsEntities.GetPendingSalesInvoices(salesInvoiceID, aspUserID, locationID, salesInvoiceTypeID, fromDate, toDate, accountInvoiceID, salesInvoiceDetailIDs).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesInvoices;
        }
    }

}
