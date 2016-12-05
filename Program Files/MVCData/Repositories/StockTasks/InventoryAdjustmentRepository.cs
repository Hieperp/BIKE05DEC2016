using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Repositories.StockTasks;



namespace MVCData.Repositories.StockTasks
{
    public abstract class InventoryAdjustmentRepository : GenericWithDetailRepository<InventoryAdjustment, InventoryAdjustmentDetail>, IInventoryAdjustmentRepository
    {
        public InventoryAdjustmentRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : this(totalBikePortalsEntities, null) { }

        public InventoryAdjustmentRepository(TotalBikePortalsEntities totalBikePortalsEntities, string functionNameEditable)
            : this(totalBikePortalsEntities, functionNameEditable, null) { }

        public InventoryAdjustmentRepository(TotalBikePortalsEntities totalBikePortalsEntities, string functionNameEditable, string functionNameApproved)
            : this(totalBikePortalsEntities, functionNameEditable, functionNameApproved, null) { }

        public InventoryAdjustmentRepository(TotalBikePortalsEntities totalBikePortalsEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable)
            : base(totalBikePortalsEntities, functionNameEditable, functionNameApproved, functionNameDeletable)
        {
        }


    }







    public class VehicleAdjustmentRepository : InventoryAdjustmentRepository, IVehicleAdjustmentRepository
    {
        public VehicleAdjustmentRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "VehicleAdjustmentEditable")
        {
        }
    }








    public class PartAdjustmentRepository : InventoryAdjustmentRepository, IPartAdjustmentRepository
    {
        public PartAdjustmentRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "PartAdjustmentEditable")
        {
        }
    }








    public class VehicleAdjustmentAPIRepository : GenericAPIRepository, IVehicleAdjustmentAPIRepository
    {
        public VehicleAdjustmentAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetVehicleAdjustmentIndexes")
        {
        }
    }

    public class PartAdjustmentAPIRepository : GenericAPIRepository, IPartAdjustmentAPIRepository
    {
        public PartAdjustmentAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetPartAdjustmentIndexes")
        {
        }
    }

    


}
