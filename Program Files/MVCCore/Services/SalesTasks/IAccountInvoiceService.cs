using System.Collections.Generic;

using MVCModel.Models;
using MVCDTO.SalesTasks;

namespace MVCCore.Services.SalesTasks
{
    public interface IAccountInvoiceService : IGenericWithViewDetailService<AccountInvoice, AccountInvoiceDetail, AccountInvoiceViewDetail, AccountInvoiceDTO, AccountInvoicePrimitiveDTO, AccountInvoiceDetailDTO>
    {
        bool Save(AccountInvoiceDTO dto, bool useExistingTransaction);
    }
}
