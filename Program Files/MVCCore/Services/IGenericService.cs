using MVCBase.Enums;
using MVCModel;
using MVCDTO;


namespace MVCCore.Services
{
    public interface IGenericService<TEntity, TDto, TPrimitiveDto>: IBaseService

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
    {
        TEntity GetByID(int id);

        bool GlobalLocked(TDto dto);
        bool Editable(TDto dto);
        bool Approvable(TDto dto);
        bool UnApprovable(TDto dto);
        bool Deletable(TDto dto);

        bool Save(TDto dto);
        bool ToggleApproved(TDto dto);
        bool Delete(int id);

        bool Alter(TDto dto);
        bool Void(int id, bool inActive);

        void PreSaveRoutines(TDto dto);
    }
}
