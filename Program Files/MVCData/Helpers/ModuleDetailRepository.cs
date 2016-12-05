using MVCCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVCModel.Models;
using MVCCore.Repositories;

namespace MVCData.Helpers
{    
    public class ModuleDetailRepository : IModuleDetailRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public ModuleDetailRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IQueryable<ModuleDetail> GetAllModuleDetails()
        {
            return this.totalBikePortalsEntities.ModuleDetails;
        }

        public IQueryable<ModuleDetail> GetModuleDetailByModuleID(int moduleID)
        {
            return this.totalBikePortalsEntities.ModuleDetails.Where(x => x.ModuleID == moduleID && x.InActive == 0);
        }

        public ModuleDetail GetModuleDetailByID(int taskID)
        {
            return this.totalBikePortalsEntities.ModuleDetails.SingleOrDefault(x => x.TaskID == taskID);
        }

        public void AddModuleDetail(ModuleDetail moduleDetail)
        {
            this.totalBikePortalsEntities.ModuleDetails.Add(moduleDetail);
        }

        public void Add(ModuleDetail moduleDetail)
        {
            this.totalBikePortalsEntities.ModuleDetails.Add(moduleDetail);
        }

        public void Remove(ModuleDetail moduleDetail)
        {
            this.totalBikePortalsEntities.ModuleDetails.Remove(moduleDetail);
        }

        public void SaveChanges()
        {
            this.totalBikePortalsEntities.SaveChanges();
        }

    }
}
