using System.Collections.Generic;

using MVCModel.Models;
using MVCDTO.SalesTasks;

namespace MVCCore.Services.SalesTasks
{
    public interface IAccountInvoiceService : IGenericWithViewDetailService<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO>
    {
        bool Save(AccountInvoiceDTO dto, bool useExistingTransaction);

        void ClearAccountInvoiceApi(int? accountInvoiceID);
        void UpdateAccountInvoiceApi(int? accountInvoiceID, int? apiSerialID, string apiSerialString, string responedMessage);

        List<AccountInvoiceSheet> GetAccountInvoiceSheet(int? accountInvoiceID);
    }
}
