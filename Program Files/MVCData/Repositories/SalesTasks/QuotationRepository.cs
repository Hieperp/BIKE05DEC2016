using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Repositories.SalesTasks;


namespace MVCData.Repositories.SalesTasks
{
    public class QuotationRepository : GenericWithDetailRepository<Quotation, QuotationDetail>, IQuotationRepository
    {
        public QuotationRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "QuotationEditable", "QuotationApproved", "QuotationDeletable")
        {
        }

        public IList<QuotationResult> GetActiveQuotations(int locationID, int? quotationID, string searchText, int isFinished)
        {
            return this.TotalBikePortalsEntities.SearchQuotations(locationID, quotationID, searchText, isFinished).ToList();
        }

    }
}
