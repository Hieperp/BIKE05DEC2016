using RequireJsNet;

using MVCCore.Services;
using MVCClient.Api.SessionTasks;


namespace MVCClient.Controllers
{
    public abstract class BaseController : CoreController
    {
        private readonly IBaseService baseService;
        public BaseController(IBaseService baseService)
        { this.baseService = baseService;}


        public IBaseService BaseService { get { return this.baseService; } }


        
        public virtual void AddRequireJsOptions()
        {
            int nmvnModuleID = this.baseService.NmvnModuleID;
            MenuSession.SetModuleID(this.HttpContext, nmvnModuleID);                

            RequireJsOptions.Add("LocationID", this.baseService.LocationID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnModuleID", nmvnModuleID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnTaskID", this.baseService.NmvnTaskID, RequireJsOptionsScope.Page);
        }

    }
}