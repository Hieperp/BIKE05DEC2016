using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.Analysis;

namespace MVCData.Repositories.Analysis
{
    public class ReportRepository: IReportRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public ReportRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public List<Report> GetReports()
        {
            return this.totalBikePortalsEntities.Reports.ToList();
        }

    }
}
