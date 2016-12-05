﻿using System;

using MVCDTO;
using MVCCore.Services.Helpers;
using MVCModel;
using MVCModel.Helpers;

namespace MVCCore.Services.Helpers
{
    public interface IHelperService<TEntity, TEntityDetail, TDto, TDtoDetail>

        where TEntity : IBaseDetailEntity<TEntityDetail>
        where TEntityDetail : class, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
        where TDto : BaseDTO, IBaseDetailEntity<TDtoDetail>
        where TDtoDetail : class, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        void GetWCParameters(TDto dto, TEntity entity, ref DateTime? checkedDate, ref string warehouseIDList, ref string commodityIDList);
    }
}
