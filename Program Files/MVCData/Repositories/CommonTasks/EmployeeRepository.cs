using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;

namespace MVCData.Repositories.CommonTasks
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public EmployeeRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<Employee> SearchEmployees(int? locationID, string searchText)
        {
            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;

            List<Employee> Employees = this.totalBikePortalsEntities.Employees.Where(w => ((int)locationID == -1976 || w.LocationID == (int)locationID) && (w.Code.Contains(searchText) || w.Name.Contains(searchText))).ToList();
            
            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return Employees;
        }
    }
}
