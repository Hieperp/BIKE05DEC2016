using System.Collections.Generic;

using MVCModel.Models;


namespace MVCCore.Repositories.SalesTasks
{
    public interface IQuotationRepository : IGenericWithDetailRepository<Quotation, QuotationDetail>
    {
        IList<QuotationResult> GetActiveQuotations(int locationID, int? quotationID, string searchText, int isFinished);
    }
}
