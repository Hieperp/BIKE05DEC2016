using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using MVCDTO.CommonTasks;
using MVCClient.ViewModels.CommonTasks;

namespace MVCClient.Api.CommonTasks
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CustomersApiController : Controller
    {
        private readonly ICustomerRepository customerRepository;        

        public CustomersApiController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }


        public JsonResult SearchSuppliersByName(string searchText)
        {
            var result = customerRepository.SearchSuppliersByName(searchText).Select(s => new { s.CustomerID, s.Name, s.AttentionName, s.Birthday, s.VATCode, s.Telephone, s.AddressNo, EntireTerritoryEntireName = s.EntireTerritory.EntireName });                    

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCustomersByName(string searchText)
        {
            var result = customerRepository.SearchCustomersByName(searchText).Select(s => new { s.CustomerID, s.Name, s.Birthday, s.VATCode, s.Telephone, s.AddressNo, EntireTerritoryEntireName = s.EntireTerritory.EntireName });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest request, int customerCategoryID, int customerTypeID, int territoryID)
        {
            var customers = this.customerRepository.SearchCustomersByIndex(customerCategoryID, customerTypeID, territoryID);

            DataSourceResult response = customers.ToDataSourceResult(request, o => new CustomerPrimitiveDTO
            {
                CustomerID = o.CustomerID,
                Name = o.Name,
                AttentionName = o.AttentionName,
                AttentionTitle = o.AttentionTitle,
                Birthday = o.Birthday,                         
                AddressNo = o.AddressNo,
                Telephone = o.Telephone,
                Facsimile = o.Facsimile,
                OfficialName = o.OfficialName,
                Remarks = o.Remarks
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomers2()
        {
            var customers = this.customerRepository.GetAllCustomers().AsQueryable();

            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomersByName(string text, [DataSourceRequest] DataSourceRequest request)
        {
            var customers = customerRepository.SearchCustomersByName(text).Select(s => new { s.CustomerID, s.Name, s.AttentionName, s.OfficialName, s.Birthday, s.Telephone, s.AddressNo, EntireTerritoryEntireName = s.EntireTerritory.EntireName });

            DataSourceResult response = customers.ToDataSourceResult(request, o => new CustomerViewModel
            {
                CustomerID = o.CustomerID,
                Name = o.Name,
                AttentionName = o.AttentionName,            
                Birthday = o.Birthday,
                AddressNo = o.AddressNo,
                Telephone = o.Telephone,                
                OfficialName = o.OfficialName,
                EntireTerritoryEntireName = o.EntireTerritoryEntireName
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}