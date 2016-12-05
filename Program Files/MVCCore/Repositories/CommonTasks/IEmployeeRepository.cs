using System.Collections.Generic;

using MVCModel.Models;

namespace MVCCore.Repositories.CommonTasks
{
    public interface IEmployeeRepository
    {
        IList<Employee> SearchEmployees(int? locationID, string searchText);
    }
}
