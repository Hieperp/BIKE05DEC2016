using MVCCore.Repositories.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Repositories.CommonTasks
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public AspNetUserRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public IList<AspNetUser> GetAllAspNetUsers()
        {
            return this.totalBikePortalsEntities.AspNetUsers.ToList();
        }
    }
}
