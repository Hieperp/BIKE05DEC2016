using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.Analysis
{
    public interface IReportRepository
    {
        List<Report> GetReports();
    }
}
