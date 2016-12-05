using System.Linq;
using System.Web.Mvc;

using MVCCore.Repositories.CommonTasks;

namespace MVCClient.Api.CommonTasks
{
    public class EmployeesApiController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesApiController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public JsonResult SearchEmployees(int? locationID, string searchText)
        {
            var result = employeeRepository.SearchEmployees(locationID, searchText).Select(s => new { s.EmployeeID, s.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchReceptionists(int? locationID, string searchText)
        {
            var result = employeeRepository.SearchEmployees(locationID, searchText).Select(s => new { ReceptionistID = s.EmployeeID, s.Name });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}