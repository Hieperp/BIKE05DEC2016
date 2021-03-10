using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel.Models;
using MVCDTO.SalesTasks;
using MVCCore.Repositories.SalesTasks;
using MVCCore.Services.SalesTasks;


namespace MVCService.SalesTasks
{
    public class AccountInvoiceService: GenericWithViewDetailService<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO>, IAccountInvoiceService
    {
        private IAccountInvoiceRepository accountInvoiceRepository;
        public AccountInvoiceService(IAccountInvoiceRepository accountInvoiceRepository)
            : base(accountInvoiceRepository, "AccountInvoicePostSaveValidate", "AccountInvoiceSaveRelative", "AccountInvoiceToggleApproved", "GetAccountInvoiceViewDetails")
        {
            this.accountInvoiceRepository = accountInvoiceRepository;
        }

        public new bool Save(AccountInvoiceDTO dto, bool useExistingTransaction)
        {
            return base.Save(dto, true);
        }

        public void ClearAccountInvoiceApi(int? accountInvoiceID, string apiAccount)
        {
            this.accountInvoiceRepository.ClearAccountInvoiceApi(accountInvoiceID, apiAccount);
        }
        
        public void UpdateAccountInvoiceApi(int? accountInvoiceID, string vATInvoiceSeries, string apiAccount, int? apiSerialID, string apiSerialString, string apiMessage)
        {
            this.accountInvoiceRepository.UpdateAccountInvoiceApi(accountInvoiceID, vATInvoiceSeries, apiAccount, apiSerialID, apiSerialString, apiMessage);
        }


        public override ICollection<AccountInvoiceViewDetail> GetViewDetails(int accountInvoiceID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("AccountInvoiceID", accountInvoiceID) };
            return this.GetViewDetails(parameters);
        }

        public List<AccountInvoiceSheet> GetAccountInvoiceSheet(int? accountInvoiceID)
        {
            return this.accountInvoiceRepository.GetAccountInvoiceSheet(accountInvoiceID);
        }
    }
}
