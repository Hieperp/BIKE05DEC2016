using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel;
using MVCDTO;

namespace MVCCore.Services
{
    public interface IGenericWithViewDetailService<TEntity, TEntityDetail, TEntityViewDetail, TDto, TPrimitiveDto, TDtoDetail> : IGenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TEntityViewDetail: class
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        ICollection<TEntityViewDetail> GetViewDetails(int id);
        ICollection<TEntityViewDetail> GetViewDetails(params ObjectParameter[] parameters);
    }
}