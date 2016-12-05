using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

using MVCCore.Helpers;
using MVCCore.Repositories;
using MVCModel.Models;

namespace MVCData.Helpers
{   
    public class ModuleRepository : IModuleRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public ModuleRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<Module> GetAllModules()
        {
            return this.totalBikePortalsEntities.Modules.Where(w => w.InActive == 0);
        }

        public Module GetModuleByID(int moduleID)
        {
            var module = this.totalBikePortalsEntities.Modules.SingleOrDefault(x => x.ModuleID == moduleID);
            return module;
        }

        //Cai nay su dung tam thoi, cho cai menu ma thoi. Cach lam nay amatuer qua!!!!
        public string GetLocationName(int userID)
        {
            var organizationalUnitUser = this.totalBikePortalsEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            return organizationalUnitUser.OrganizationalUnit.Location.OfficialName;
        }
        public int GetLocationID(int userID)
        {
            var organizationalUnitUser = this.totalBikePortalsEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            return organizationalUnitUser.OrganizationalUnit.Location.LocationID;
        }






        public void SaveChanges()
        {
            this.totalBikePortalsEntities.SaveChanges();
        }

        public void Add(Module module)
        {
            this.totalBikePortalsEntities.Modules.Add(module);
        }

        public void Delete(Module module)
        {
            this.totalBikePortalsEntities.Modules.Remove(module);
        }
    }
}
