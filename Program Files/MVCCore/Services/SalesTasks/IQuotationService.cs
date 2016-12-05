using MVCModel.Models;
using MVCDTO.SalesTasks;

namespace MVCCore.Services.SalesTasks
{
    public interface IQuotationService : IGenericWithViewDetailService<Quotation, QuotationDetail, QuotationViewDetail, QuotationDTO, QuotationPrimitiveDTO, QuotationDetailDTO>
    {
    }
}
