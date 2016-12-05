using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel.Models;
using MVCDTO.SalesTasks;
using MVCCore.Repositories.SalesTasks;
using MVCCore.Services.SalesTasks;

namespace MVCService.SalesTasks
{
    public class QuotationService : GenericWithViewDetailService<Quotation, QuotationDetail, QuotationViewDetail, QuotationDTO, QuotationPrimitiveDTO, QuotationDetailDTO>, IQuotationService
    {
        public QuotationService(IQuotationRepository quotationRepository)
            : base(quotationRepository, "QuotationPostSaveValidate", "QuotationSaveRelative", null, "GetQuotationViewDetails")
        {
        }

        public override ICollection<QuotationViewDetail> GetViewDetails(int quotationID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("QuotationID", quotationID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(QuotationDTO quotationDTO)
        {
            quotationDTO.QuotationViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(quotationDTO);
        }

    }


}
