using MVCBase.Enums;

namespace MVCDTO
{
    public interface IPrimitiveDTO
    {
        GlobalEnums.NmvnTaskID NMVNTaskID { get; }
        void SetID(int id);
    }
}
